# 线点云 
"""
画一个任意线段点云, 加噪 噪声在noise中
可以调节 
    - 采样步距 
    - 点云参数 t 范围
    - 直线方程 X = X0+kx*t, Y = Y0+ky*t, Z = Z0+kz*t
    - 噪声大小 或 噪声类型
"""
step = 0.1             # 步距
tlim = [-4, 10]          # 参数 t 范围
X0, Y0, Z0 = 0, 0, 0    # 参数式方程系数
kx, ky, kz = 1, 1, 1

from matplotlib import pyplot as plt
import numpy as np
from mpl_toolkits.mplot3d import Axes3D

t = np.arange(tlim[0], tlim[1], step)       # 生成点云 t 坐标 列
X = X0 + kx * t
Y = Y0 + ky * t
Z = Z0 + kz * t 

noise = (np.random.rand(t.shape[0])-0.5) * 0.1       # 加噪 [-0.5, 0.5) 噪声
Zn = Z + noise

# 存储数据点
PC = np.dstack((X, Y, Zn))
PCs = PC.reshape((t.shape[0],-1))
np.savetxt("E:/LinePC.txt", PCs, delimiter=',', fmt="%f")  

# 画图
fig = plt.figure()                          
ax = Axes3D(fig)
ax.scatter(X, Y, Zn, c='r')  # 绘制数据点
ax.set_zlabel('Z')           # 坐标轴
ax.set_ylabel('Y')
ax.set_xlabel('X')
plt.show()
