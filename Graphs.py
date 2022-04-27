import matplotlib.pyplot as plot
import numpy as np
from scipy.special import erf
from scipy.optimize import curve_fit
from CalcsOnDatasets import normalize, define_dfs

def fit(x, y, f):
    popt, _ = curve_fit(f, x, y)
    # y_line = f(x_future, *popt)
    func = lambda x: f(x, *popt)

    return (popt, func)

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
        value.append(np.interp(R_critical, yvalues,xvalues)) 
        
        # get Rs
        # value.append(np.interp(etas[etaidx], xvalues, yvalues))
        # etaidx += 1

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
    
    # etas = [ # 5,8,10,12,16,20,24,32,40,48,64,80,90,100
    #     # 1.0924358879439666, 
    #     # 1.1210694783648496, 
    #     # 1.1200673539564878, 
    #     # 1.1242160598362503, 
    #     1.1276616803293817, #
    #     1.127161183386989, 
    #     1.1269442754295838, 
    #     1.1295837537191724, #
    #     1.1282770466025258, 
    #     1.1283937712477239, 
    #     1.1285683032741898, #
    #     1.1283650180842508, 
    #     1.1278711227869513, 
    #     1.128457740043094
    # ]

    # Ls_scaled = [L**(-1.93582 - 3/4) for L in Ls]

    func_fit = lambda x,p,a,b: p + a*x**(-b)
    # popt, f = extrapolate(Ls_scaled, etas, fit, 0, 8*10**(-4), 10**(-5))
    # f = extrapolate(Ls, etas, fit, min(Ls) - 10, max(Ls) + 10, 1)

    x_range = np.arange(0, max(Ls) + 100, 1)
    coef, f = fit(Ls, etas, func_fit)
    etas_lowered = [coef[0] - eta for eta in etas]
    plot.plot(Ls, etas_lowered, "o", x_range, coef[0] - f(x_range), "-")

    print(coef)


    plot.xlim(10, 600)
    # plot.ylim(1.1278, 1.1288)
    # plot.yticks(np.arange(1.1278, 1.1288, 0.0001))
    # plot.minorticks_on()
    plot.tick_params(which='both', width=1, labelsize=30)
    plot.tick_params(which='major', length=9)
    plot.tick_params(which='minor', length=4)
    plot.xscale("log")
    plot.yscale("log")
    plot.xlabel("L", fontsize=35)
    plot.ylabel("$\eta_L- \eta_\infty$", fontsize=33)
    plot.errorbar(Ls, etas_lowered, yerr=uncertainties, fmt='o',ecolor = 'black',color='black')
    plot.show()

def extrapolate(x, y, func, x_start, x_stop, x_step):
    x_range = np.arange(x_start, x_stop, x_step)
    popt, f = fit(x, y, func)
    plot.plot(x, y, 'o', x_range, f(x_range), '-')
    
    return f

def R_infty_minus_R_L(Ls):

    # R_above_eta = [ # 5,10,16,20,32,40,64,80,90,100
    #     0.722530, 
    #     0.696274, 
    #     0.685804, 
    #     0.687107, 
    #     0.681242, 
    #     0.685690, 
    #     0.684740, 
    #     0.684126, 
    #     0.690037, 
    #     0.682781
    # ] # according to fit

    # R_above_eta = [ # 5,10,16,20,32,40,64,80,90,100
    #     0.736747, 
    #     0.705400, 
    #     0.702649, 
    #     0.693790, 
    #     0.690146, 
    #     0.692428, 
    #     0.693929, 
    #     0.682830, 
    #     0.687378, 
    #     0.685125, 
    #     0.682039, 
    #     0.682894, 
    #     0.688136, 
    #     0.680107
    # ] # discrete

    R_above_eta = [ # 5,10,16,20,32,40,64,80,90,100
        0.7375063257449366, 
        0.7065466320311007, 
        0.7030715316518507, 
        0.6942830959131691, 
        0.6921492949160272, 
        0.6947629850219126, 
        0.6950131114035678, 
        0.6838660641101124, 
        0.6888990855749351, 
        0.6891639332679035, 
        0.6863359871844703, 
        0.6877740626714242, 
        0.6934130049145162, 
        0.6860547964419628
    ] # previous eta = 1.12808

    delta = [R_l - R_critical for R_l in R_above_eta]
    x_future = np.arange(0,120,.1)

    x, y = fit(Ls, delta, lambda x, a: x**(a), x_future)
    plot.xscale('log')
    plot.yscale('log')
    plot.tick_params(which='both', width=1, labelsize=33)
    plot.tick_params(which='major', length=9)
    plot.tick_params(which='minor', length=4)
    plot.xlabel("L", fontsize=35)
    plot.ylabel("$R_L(\eta_C)-R_\infty(\eta_C)$", fontsize=33)
    plot.plot(Ls, delta, 'o', x, y, '-')
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
    uncertainties = [0.004623225884996106, 0.0046558476044233086, 0.004714890308454124, 0.004782799177513486, 0.004896995456274903, 0.004998565976813291]
    # Ls = [5,8,10,12,16,20,24,32,40,48,64,80,90,100]
    # Ls = [16,20,24,32,40,48,64,80,90,100]
    BLACK = ["black" for _ in range(len(Ls))]

    Rs = define_dfs("R_L", Ls, ["n", "cdf", "eta"])
    # plot_dfs(Rs, BLACK)
    plot_fits(Rs, BLACK)
    # eta_C(Ls)

    # print(interpolate_Rs(Rs, "x"))
    # R_infty_minus_R_L(Ls)