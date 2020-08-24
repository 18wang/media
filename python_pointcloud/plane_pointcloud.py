# 平面点云 
"""
画一个任意平面点云, 加噪 噪声在noise中
可以调节 
    - X, Y步距 
    - 点云 X, Y 范围
    - 平面方程 Z = f(X, Y)
    - 噪声大小 或 噪声类型
"""
step = 0.25             # 步距
xlim = [-4, 10]          # 坐标范围
ylim = [-4, 10]

from matplotlib import pyplot as plt
import numpy as np
from mpl_toolkits.mplot3d import Axes3D

X = np.arange(xlim[0], xlim[1], step)       # 生成点云 x坐标 列
Y = np.arange(ylim[0], ylim[1], step)
X, Y = np.meshgrid(X, Y)                    # 生成网格
Z = X+Y # np.zeros_like(X)

x, y = Z.shape                              # 加噪 [-0.5, 0.5) 噪声
noise = (np.random.rand(x, y)-0.5) * 0.1 
Zn = Z + noise

# 存储数据点
PC = np.dstack((X, Y, Zn))
PCs = PC.reshape((x*y,-1))
np.savetxt("E:/PlanePC.txt", PCs, delimiter=',', fmt="%f")

# 画图
fig = plt.figure()                          
ax = Axes3D(fig)
# 具体函数方法可用 help(function) 查看，如：help(ax.plot_surface)
ax.plot_surface(X, Y, Zn, rstride=1, cstride=1, cmap='rainbow')
ax.set_zlabel('Z')          # 坐标轴
ax.set_ylabel('Y')
ax.set_xlabel('X')
plt.show()