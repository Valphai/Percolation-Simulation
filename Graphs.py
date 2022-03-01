import numpy as np
import pandas as pd
import matplotlib.pyplot as plot
from scipy.special import erf
from scipy.optimize import curve_fit

RPDF16 = "Rpdf=16_a=0.6.dat"
RPDF32 = "Rpdf=32_a=0.6.dat"
RPDF64 = "Rpdf=64_a=0.6.dat"
CDF16 = "CDF_L=16_a=0.6.dat"
CDF32 = "CDF_L=32_a=0.6.dat"
CDF64 = "CDF_L=64_a=0.6.dat"

rpdf16 = pd.read_csv(RPDF16, "\t")
rpdf32 = pd.read_csv(RPDF32, "\t")
rpdf64 = pd.read_csv(RPDF64, "\t")
cdf16 = pd.read_csv(CDF16, "\t")
cdf32 = pd.read_csv(CDF32, "\t")
cdf64 = pd.read_csv(CDF64, "\t")

def plot_fits(df, colors):
    
    def objective(x, b, c):
        return erf(b * x + c)

    def fit(f, col):
        f.columns = ["n", "cdf", "eta"]
        x = f["eta"]
        y = f["cdf"]
        popt, _ = curve_fit(objective, x, y)
        # x_line = np.arange(min(x), max(x), 1)
        # calculate the output for the range
        y_line = objective(x, *popt)
        # create a line plot for the mapping function
        return plot.plot(x, y_line, color=col)
        # plot.show()

    ax = fit(df[0], colors[0])
    for i, data in enumerate(df[1:], 1):
        fit(data, colors[i])

    plot.show()
        
def plot_cpdfs(cdfs, colors):
    def plot_func(f, col, ax=None):
        f.columns = ["n", "cdf", "eta"]
        # f["etaC"] = f["n"] * a / (L * L)
        return f.plot.scatter(x="eta", y="cdf", color=col, ax=ax)

    ax = plot_func(cdfs[0], colors[0])
    for i, r_pdf in enumerate(cdfs[1:], 1):
        plot_func(r_pdf, colors[i], ax)

    plot.show()

def plot_rpdfs(r_pdfs, colors):
    def plot_func(f, col, ax=None):
        f.columns = ["n", "R", "eta"]
        # f["R"] = f["R"].cumsum()
        # f["R"] = (f["R"]-f["R"].min())/(f["R"].max()-f["R"].min())
        return f.plot.scatter(x="eta", y="R", color=col, ax=ax)

    ax = plot_func(r_pdfs[0], colors[0])
    for i, r_pdf in enumerate(r_pdfs[1:], 1):
        plot_func(r_pdf, colors[i], ax)

    plot.show()

def normalize(column):
    return (column-column.min())/(column.max()-column.min())

plot_rpdfs([rpdf16, rpdf32, rpdf64], ["blue", "green", "red"])
plot_cpdfs([cdf16, cdf32, cdf64], ["blue", "green", "red"])
plot_fits([cdf16, cdf32, cdf64], ["blue", "green", "red"])