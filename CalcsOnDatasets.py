import math
from statistics import mean
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

def define_df(prefix, L, cols):
    df = pd.read_csv(f"data\{prefix}={L}_a={r}.dat", "\t")
    df.columns = cols
    return df

def define_dfs(prefix, Ls, cols):
    new_cdfs = []
    for L in Ls:
        new_cdfs.append(define_df(prefix, L, cols))

    return new_cdfs

def normalize(column):
    return (column-column.min())/(column.max()-column.min())

def assign_R(cdfs, Ls):
    def assign_R(df, L):
        RealL = L * 2 * r
        df["cdf"] = df.apply(
            lambda row: smooth_with_poisson(df.cdf, row["eta"], r, RealL, row.name),
            axis=1
        )

    for i, df in enumerate(cdfs):
        assign_R(df, Ls[i])

def save_to_file(cdfs, Ls):
    def save(df, L):
        df.to_csv(f"R_L={L}_a={r}.dat", header=None, index=None, sep="\t")

    for i, df in enumerate(cdfs):
        save(df, Ls[i])

def uncertainties(Ls, Ns):
    # formula = lambda R, N: math.sqrt((R * (1 - R)) / (N))
    formula = lambda L, N : N**(-1/2)*L**(-3/4)
    return [formula(L, N) for L, N in list(zip(Ls, Ns))]

def ux(x):
    n = len(x)
    diff = [i - mean(x) for i in x]
    squared_numbers = [x**2 for x in diff]
    return math.sqrt((1/(n*(n-1)))*sum(squared_numbers))

if __name__ == "__main__":
    Ls = [16,32,64,128,256,512]
    Rs = [0.6904709909162112, 0.6824121286474356, 0.6664942160649269, 0.6458464790241288, 0.6010857832572333, 0.5129756470463123]
    etas = [ # y 16,32,64,128,256,512
        1.123810661285237, 
        1.1254063066419935, 
        1.12651491897615, 
        1.1268276086098574, 
        1.1271262729482818, 
        1.12751513652192
    ]
    print(mean(etas))