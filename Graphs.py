import pandas as pd
import matplotlib.pyplot as plot
from scipy.special import erf
from scipy.optimize import curve_fit

a = .6

RPDF16 = f"Rpdf=16_a={a}.dat"
RPDF32 = f"Rpdf=32_a={a}.dat"
RPDF64 = f"Rpdf=64_a={a}.dat"
RPDF128 = f"Rpdf=128_a={a}.dat"
RPDF256 = f"Rpdf=256_a={a}.dat"
RPDF512 = f"Rpdf=512_a={a}.dat"
CDF16 = f"CDF_L=16_a={a}.dat"
CDF32 = f"CDF_L=32_a={a}.dat"
CDF64 = f"CDF_L=64_a={a}.dat"
CDF128 = f"CDF_L=128_a={a}.dat"
CDF256 = f"CDF_L=256_a={a}.dat"
CDF512 = f"CDF_L=512_a={a}.dat"

rpdf16 = pd.read_csv(RPDF16, "\t")
rpdf32 = pd.read_csv(RPDF32, "\t")
rpdf64 = pd.read_csv(RPDF64, "\t")
rpdf128 = pd.read_csv(RPDF128, "\t")
rpdf256 = pd.read_csv(RPDF256, "\t")
rpdf512 = pd.read_csv(RPDF512, "\t")
cdf16 = pd.read_csv(CDF16, "\t")
cdf32 = pd.read_csv(CDF32, "\t")
cdf64 = pd.read_csv(CDF64, "\t")
cdf128 = pd.read_csv(CDF128, "\t")
cdf256 = pd.read_csv(CDF256, "\t")
cdf512 = pd.read_csv(CDF512, "\t")

BLACK = ["black" for _ in range(7)]
COLORS = ["blue", "green", "red", "black", "pink", "orange"]
TOPLOTR = [rpdf16, rpdf32, rpdf64, rpdf128, rpdf256, rpdf512]
TOPLOT = [cdf16, cdf32, cdf64, cdf128, cdf256, cdf512]

def plot_fits(df, colors):
    
    def objective(x, a, b, c, d):
        return erf(a*x**c - b)/a + d

    def fit(f, col, x_future):
        f.columns = ["n", "R", "eta"]
        x = f["eta"]
        y = f["R"]
        popt, _ = curve_fit(objective, x, y)
        y_line = objective(x_future, *popt)
        y_normalized = normalize(y_line)
        return plot.plot(x_future, y_normalized, color=col)

    df[0].columns = ["n", "R", "eta"]
    x_future = df[0]["eta"]

    for i, data in enumerate(df):
        fit(data, colors[i], x_future)

    plot.show()
    
def plot_dfs(cdfs, colors):
    def plot_func(f, col, ax=None):
        f.columns = ["n", "R", "eta"]
        return f.plot.scatter(x="eta", y="R", color=col, ax=ax)

    ax = plot_func(cdfs[0], colors[0])
    for i, df in enumerate(cdfs[1:], 1):
        plot_func(df, colors[i], ax)

    plot.show()

def normalize(column):
    return (column-column.min())/(column.max()-column.min())

plot_dfs(TOPLOT, BLACK)
plot_fits(TOPLOT, BLACK)