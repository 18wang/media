# 圆柱侧面点云 
"""
画一个椭圆柱侧面点云, 以 z轴 为轴心, 向正负方向延申
加噪 噪声在noiseX, noiseY中
可以调节 
    - xoy 截面的采样角度步距 0~2π, z 轴方向的采样步距 
    - 点云 z轴 范围
    - 椭圆正截面长短轴 (x/a)^2 + (y/b)^2 = 1
    - 噪声大小 或 噪声类型
"""
stepz = 0.2            # z轴方向步距  
stepA = 0.1             # xoy平面角度步距
zlim = [-4, 1]          # z坐标范围
a, b = 1, 1             # 长轴短轴

from matplotlib import pyplot as plt
import numpy as np
from mpl_toolkits.mplot3d import Axes3D

X = a * np.cos(np.arange(0,2*np.pi,stepA))          # 生成坐标
Y = b * np.sin(np.arange(0,2*np.pi,stepA))
Z = np.arange(zlim[0], zlim[1], stepz)
X, _ = np.meshgrid(X, Z)  
Y, Z = np.meshgrid(Y, Z)  

shape0, shape1 = X.shape[0], X.shape[1]
noiseX = (np.random.rand(shape0, shape1)-0.5) * 0.05   # 加噪 [-0.25, 0.25) 噪声
noiseY = (np.random.rand(shape0, shape1)-0.5) * 0.05    # X Y 均加噪
Xn = X + noiseX
Yn = Y + noiseY

# 存储数据点
PC = np.dstack((Xn, Yn, Z))
PCs = PC.reshape((shape0*shape1,-1))
np.savetxt("E:/CylinderPC.txt", PCs, delimiter=',', fmt="%f")

# 画图
fig = plt.figure()                          
ax = Axes3D(fig)
# 具体函数方法可用 help(function) 查看，如：help(ax.plot_surface)
ax.plot_surface(Xn, Yn, Z, rstride=1, cstride=1, cmap='rainbow')
ax.set_zlabel('Z')           # 坐标轴
ax.set_ylabel('Y')
ax.set_xlabel('X')
plt.show()