import matplotlib.pyplot as plot
import numpy as np
from scipy.special import erf
from scipy.optimize import curve_fit
from CalcsOnDatasets import normalize, define_dfs

def fit(x, y, f):
    popt, pcov = curve_fit(f, x, y)
    func = lambda x: f(x, *popt)
    perr = np.sqrt(np.diag(pcov))

    return (popt, func, perr)

def plot_fits(df, colors):
    
    def objective(x, a, b, c, d):
        return erf(a*x**c - b)/a + d

    def fit_df(f, col, x_future, value, etaidx):
        f.columns = ["n", "R", "eta"]
        x = f["eta"]
        y = f["R"]
        _, y_line = fit(x, y, objective)
        y_normalized = normalize(y_line(x_future))

        line2d = plot.plot(x_future, y_normalized, color=col)
        
        yvalues = line2d[0].get_ydata()
        xvalues = line2d[0].get_xdata()

        # get etas
        # value.append(np.interp(R_critical, yvalues,xvalues)) 
        
        # get Rs
        value.append(np.interp(etas[etaidx], xvalues, yvalues))
        etaidx += 1

        return line2d

    value = []
    etas = [ # y 16,32,64,128,256,512
        1.12381,
        1.12540,
        1.12651,
        1.12682,
        1.12712,
        1.12751
    ]
    etaidx = 0

    df[0].columns = ["n", "R", "eta"]
    x_future = df[0]["eta"]

    for i, data in enumerate(df):
        fit_df(data, colors[i], x_future, value, etaidx)

    # print(sum(value)/len(value)) # srednia arytm
    print(value)
    plot.tick_params(axis="both", which="major", labelsize=35)
    plot.plot([0.675, 1.51], [R_critical, R_critical], color='black', linestyle="--", linewidth=1)
    plot.xlabel("η", fontsize=35)
    plot.ylabel("R(η)", fontsize=35)
    plot.show()

def plot_dfs(cdfs, colors):
    def plot_func(f, col, ax=None):
        return f.plot.scatter(x="eta", y="cdf", color=col, ax=ax)

    ax = plot_func(cdfs[0], colors[0])
    for i, df in enumerate(cdfs[1:], 1):
        plot_func(df, colors[i], ax)

    plot.show()

def eta_C(Ls):

    etas = [ # y 16,32,64,128,256,512
        1.12381,
        1.12540,
        1.12651,
        1.12682,
        1.12712,
        1.12751
    ]

    func_fit = lambda x,p,a,b: p + a*x**(-b)

    x_range = np.arange(0, max(Ls) + 100, 1)
    coef, f, perr = fit(Ls, etas, func_fit)
    etas_lowered = [coef[0] - eta for eta in etas]
    plot.plot(Ls, etas_lowered, "o", x_range, coef[0] - f(x_range), "-")

    print(coef, perr)

    plot.xlim(10, 600)
    plot.tick_params(which='both', width=1, labelsize=30)
    plot.tick_params(which='major', length=9)
    plot.tick_params(which='minor', length=4)
    plot.xscale("log")
    plot.yscale("log")
    plot.xlabel("L", fontsize=35)
    plot.ylabel("$\eta_L- \eta_\infty$", fontsize=33)
    plot.errorbar(Ls, etas_lowered, yerr=sigma_eta_L, fmt='o',ecolor = 'black',color='black')
    plot.show()

def extrapolate(x, y, func, x_start, x_stop, x_step):
    x_range = np.arange(x_start, x_stop, x_step)
    popt, f, _ = fit(x, y, func)
    plot.plot(x, y, 'o', x_range, f(x_range), '-')
    
    return f

def infty_minus(Ls, ys, y_critical, y_label):

    delta = [y_critical - y for y in ys]
    x_future = np.arange(0,120,.1)

    generic_log_fit(
        Ls, delta, 
        "L", y_label, x_future
    )

def generic_log_fit(x, y, x_label, y_label, x_future=None):
    popt, f, _ = fit(x, y, lambda x, a: x**(a))
    plot.xscale('log')
    plot.yscale('log')
    plot.tick_params(which='both', width=1, labelsize=33)
    plot.tick_params(which='major', length=9)
    plot.tick_params(which='minor', length=4)
    plot.xlabel(x_label, fontsize=35)
    plot.ylabel(y_label, fontsize=33)
    
    plot.plot(x, y, 'o', x, f(x), '-')
    
    plot.show()

def interpolate_points(x, y, axis):
    line2d = plot.plot(x, y)
        
    yvalues = line2d[0].get_ydata()
    xvalues = line2d[0].get_xdata()

    if (axis == "x"): # get etas
        return np.interp(R_critical, yvalues,xvalues)
    else: # get Rs
        return np.interp(eta_critical, xvalues, yvalues)

def interpolate_Rs(Rs, axis):
    results = []
    for Rdf in Rs:
        x, y = Rdf["eta"], Rdf["cdf"]
        results.append(interpolate_points(x, y, axis))
    
    return results

if __name__ == "__main__":
    
    R_critical = 0.690473
    eta_critical = 1.12758
    Ls = [16,32,64,128,256,512]
    etas = [1.12381, 1.12540, 1.12651, 1.12682, 1.12712, 1.12751] # y 16,32,64,128,256,512
    BLACK = ["black" for _ in range(len(Ls))]
    
    sigma_R_L = [0.004623225884996106, 0.0046558476044233086, 0.004714890308454124, 0.004782799177513486, 0.004896995456274903, 0.004998565976813291]
    # sigma_eta_L = [1.4/(L**(3/4)*9999**(1/2)) for L in Ls]
    sigma_eta_L = [0.0017500875065630467, 0.0010406082573410733, 0.0006187493717802934, 0.00036791057766229446, 0.00021876093832038084, 0.00013007603216763416]

    eta_C(Ls)