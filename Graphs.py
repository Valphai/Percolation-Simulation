import pandas as pd
import matplotlib.pyplot as plot
import math

RPDF16 = "Rpdf=16_a=0.6.dat"
RPDF32 = "Rpdf=32_a=0.6.dat"
PDF16 = "PDF_L=16_a=0.6.dat"
CDF16 = "CDF_L=16_a=0.6.dat"

rpdf16 = pd.read_csv(RPDF16, "\t")
rpdf32 = pd.read_csv(RPDF32, "\t")
pdf16 = pd.read_csv(PDF16, "\t")
cdf16 = pd.read_csv(CDF16, "\t")
pdf16.columns = ["n", "pdf"]

COLORS = ["blue", "purple"]
RPDFS = [rpdf16, rpdf32]

L = 16
a = .6


def plot_funcs(r_pdfs, colors):
    def plot_func(f, col, ax=None):
        f.columns = ["n", "R", "eta"]
        # f["etaC"] = f["n"] * math.pi * .6 * .6 / ((L*i) * (L*i))
        # f["R"] = f["R"].cumsum()
        # f["R"] = (f["R"]-f["R"].min())/(f["R"].max()-f["R"].min())
        return f.plot.scatter(x="eta", y="R", color=col, ax=ax)

    ax = plot_func(r_pdfs[0], colors[0])
    for i, r_pdf in enumerate(r_pdfs[1:], 1):
        plot_func(r_pdf, colors[i], ax)

    plot.show()

def normalize(column):
    return (column-column.min())/(column.max()-column.min())

plot_funcs(RPDFS, COLORS)

# pdf16["pdf"] = normalize(pdf16["pdf"])
# pdf16.plot.scatter(x="n", y="pdf")
# plot.show()
