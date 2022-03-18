import matplotlib.pyplot as plot
from scipy.special import erf
from scipy.optimize import curve_fit
from CalcsOnDatasets import normalize, define_dfs

BLACK = ["black" for _ in range(7)]
COLORS = ["blue", "green", "red", "black", "pink", "orange"]

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
        return f.plot.scatter(x="eta", y="R", color=col, ax=ax)

    ax = plot_func(cdfs[0], colors[0])
    for i, df in enumerate(cdfs[1:], 1):
        plot_func(df, colors[i], ax)

    plot.show()

if __name__ == "__main__":
    Rs = define_dfs("R_L")
    plot_dfs(Rs, BLACK)
    plot_fits(Rs, BLACK)