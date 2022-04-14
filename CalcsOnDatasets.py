import math
import pandas as pd

r = 0.6

def smooth_with_poisson(values, eta, disc_radius, L, indx_in_vals):
    
    def S(r):
        return math.pi * r * r

    # Eq 3 in Mertens Moore PRE, "Continuum Percolation Thresholds in Two Dimensions"
    RealL = L * 2 * r
    lambd = eta * RealL * RealL / S(disc_radius) # this is n
    result = 0.0
    weight_sum = 0.0
    n_hat = int(math.floor(lambd))

    # left
    weight = 1.0
    for i in range(indx_in_vals, 0, -1):

        k = indx_in_vals - i
        result += weight * values[i]
        
        weight_sum += weight
        weight *= (n_hat - k) / lambd

    # right
    weight = 1.0
    for i in range(indx_in_vals, len(values)):
        
        k = i - indx_in_vals
        result += weight * values[i]
        
        weight_sum += weight
        weight *= lambd / (n_hat + k + 1)

    result /= weight_sum
    return result

def define_dfs(prefix):
    def define_df(L):
        df = pd.read_csv(f"{prefix}={L}_a={r}.dat", "\t")
        df.columns = ["n", "cdf", "eta"]
        return df

    L = 16
    new_cdfs = []
    for _ in range(1, 7):
        new_cdfs.append(define_df(L))
        L *= 2

    return new_cdfs

def normalize(column):
    return (column-column.min())/(column.max()-column.min())

def assign_R(cdfs):
    def assign_R(df, L):
        RealL = L * 2 * r
        df["cdf"] = df.apply(
            lambda row: smooth_with_poisson(df.cdf, row["eta"], r, RealL, row.name),
            axis=1
        )

    L = 16
    for df in cdfs:
        assign_R(df, L)
        L *= 2

def save_to_file(cdfs):
    def save(df, L):
        df.to_csv(f"R_L={L}_a={r}.dat", header=None, index=None, sep="\t")

    L = 16
    for df in cdfs:
        save(df, L)
        L *= 2


if __name__ == "__main__":
    cdfs = define_dfs("CDF_L")
    assign_R(cdfs)
    save_to_file(cdfs)