[Percolation Threeshold](https://en.wikipedia.org/wiki/Percolation_threshold) - number of disks necessary for percolation to occur.

Critical wrapping probability - prawdopodobieństwo wystąpienia wrappingu w równowadze termodynamicznej, stanie w którym funkcje stanów przyjmują wartości stałe, czyli $\forall$ L dostajemy to samo pdp.

[finite size scaling](http://mis.kp.ac.rw/admin/admin_panel/kp_lms/files/digital/SelectiveBooks/Physics/An%20Introduction%20to%20Computer%20Simulation%20Methods.pdf) - teoria do obliczenia $L^\frac{-11}{4}$ s 480

DONE
Monte Carlo Simulation - model, w którym zmiany nie zachodzą według wcześniej ustalonych sekwencji, zaś w sposób, uzależniony od liczb losowych, które generowane są w trakcie symulacji. W ten sposób wielokrotnie uruchamiane symulacje nie skutkują identycznymi wynikami ale wynikami, które zgadzają się z poprzednimi co do "błędu statystycznego".

DONE
Próg perkolacji - krytyczny zbiór obiektów, dla których istnieje "nieskończony perkolujący klaster" (eng. infinite percolating cluster) \citep{}. Perkolujący klaster to taki, który zawija sieć. W omawianym przypadku przekłada się to na ilość dysków, potrzebną do pojawienia się zawijanego klastra. 
% s19

___

L = plane width and height
n = number of penetrable objects (disks)
ρ = mean density = n/L^2 (lambda, intensity)
a = object area
η = ρa = filling factor
φ = 1 − e^−η = total fraction of the plane covered by the objects
representative = cluster parent

R_L(η) = wrapping probability functions 
R^e_L(η) = the probability of any kind of wrapping cluster. 
R^h_L(η) = the probability of a cluster that wraps horizontally.
R^1_L(η) = the probability of a cluster that wraps horizontally, but not vertically
R^b_L(η) = the probability of a cluster that wraps both horizontally and vertically

horizontal and vertical wrapping probabilities are equal

disks are thrown at random in the L square (Poisson point process)

take a look at grid bin AddDisk, gotta fiddle with union find now

they do both splitting and compression?


sum down all to smallest n => P_L(a, n) is non zero
sum up all to highest n => P_L(a, n) is 1


for each simulation calculate ρ, then calculate η = ρa 

THEN SUM THESE BOYS UP IN (3)

(5) is essentially saying what critical factor η_L gives R_L = 1

Problem now is no periodic boundaries, when summing path 
cut part of the disk at the edge should be on the opposite side of the plane