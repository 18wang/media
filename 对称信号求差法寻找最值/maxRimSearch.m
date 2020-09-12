%{
考虑使用光强的对称性, 最高点处光信号突变独特很难用滤波和拟合恢复,
 两侧存在大量信号, 且它们的噪声比较接近, 适合用作比较
1.计算 Res(k) = i 属于 (1, windowSize) Σ(X(k+i)-X(k-i))^2 , k = 1, 2, ...  
2. 注意到 Res 图像, 呈 M 型, 完美的对称图形中间的局部最小值应该为0, 该点位置即为我们要求取的点, 取两边光滑且对称的点
    a. 设定一个门限, 计算相同y值对应的横坐标x的平均数, 即为所求的x 
    b. 拟合两条曲线, 交点即为x, 且两条曲线理论上关于y轴对称; 或者将两条曲线 放在一个场景中进行拟合, 最高点为x. 
    c. M 中间的局部最小值即对应着光强最大点
3. 后续处理
%}

%% 数据读取
fileName = "E:/Temp/9-9/15.txt";
data = csvread(fileName);       
rim = data(:, 2);           % 表格第二列

%% 窗口宽度确定
Len = length(rim);
half = fix(Len/2);
[Max, LMax] = max(rim);             % 光强最大值
index50 = MaxPercent(rim, Max*0.40, 20);
windowSize = 1 * (index50(end) - index50(1));   % 窗口宽度和那几个值的最外界相关  窗口长度的选择比较重要
if(windowSize<Len/20)
    windowSize = fix(Len/20);
end
if( LMax - 3*windowSize < 1 ) || (LMax + 3*windowSize > Len)
    windowSize = fix(windowSize/2); 
end
% windowSize = 917;
Res = zeros(Len,1);
figure(1);
subplot(211);
plot( data(:, 1), rim);
text(data(LMax,1), Max, num2str(data(LMax,1)) );

%% 计算窗口左右两侧 Res, 差的平方
for i = ceil(LMax-2*windowSize) : ceil(LMax+2*windowSize)
    diff = rim(i: -1: i-windowSize+1) - rim(i+1:i+windowSize);
    res = sum(diff.^2)/windowSize;
    Res(i+1) = res;
end
% figure(2);

subplot(212);
plot(data(:, 1), Res, 'm');
hold on;

%% a采用平均数, 计算中心位置 
% 取两侧光滑曲线 找对应点
lowGate = 0.3;     % 低点处门限
upGate = 0.70;      % 高点处门限
[MaxR, MRL] = max(Res);
indexlow = MaxPercent(Res, MaxR*lowGate, 10);    % 求取 n个 位于门限附近的点, 取最外侧作为门限
indexup = MaxPercent(Res, MaxR*upGate, 10);
indexlowL = indexlow(1);                        % 取最外侧两个做为侧边, 可能存在风险
indexlowR = indexlow(end);
indexupL = indexup(1);
indexupR = indexup(end);

% indexlowL = MaxPercent(Res(MRL-2*windowSize:MRL), MaxR*lowGate, 1) + MRL-2*windowSize-1;
% indexupR = MaxPercent(Res(MRL+windowSize*1.5:3*windowSize+MRL), MaxR*upGate, 1) + MRL-1+windowSize*1.5;
% indexlowR = MaxPercent(Res(MRL:2*windowSize+MRL), MaxR*lowGate, 1) + MRL-1;
% indexupL = MaxPercent(Res(MRL-2*windowSize:MRL), MaxR*upGate, 1) + MRL-2*windowSize-1;

% plot(indexlowL, Res(indexlowL), 'ko');
% plot(indexlowR, Res(indexlowR), 'b^');
% plot(indexupL, Res(indexupL), 'ko');
% plot(indexupR, Res(indexupR), 'b^');
% hold on;

result = ceil(mean([indexlowL, indexlowR, indexupL, indexupR]));     % 坐标均值 结果  
R1 = data(result,1);

%% b采用拟合, 计算中心位置
LeftRes =Res(indexlowL: indexupL);
RightRes =Res(indexupR: indexlowR);
x1 = [indexlowL: indexupL]';
% fL=fit(x1, LeftRes,'rat42');          % 分别拟合, 拟合函数应当对称
% plot(fL, x1, LeftRes);
x2 = [indexupR: indexlowR]';
% fR=fit(x2, RightRes,'fourier3');
% plot(fR, x2, RightRes);
% f=fit([x1; x2], [LeftRes; RightRes],'fourier1');    % 或者总体拟合  fourier1 gauss1 poly234 rat41

% plot(f, [x1; x2], [LeftRes; RightRes]);
% hold on;

% [M, ML] = max(f(indexlowL:indexlowR));          % 求取 拟合函数 最大值点 结果
% ML = ML + indexlowL-1;
% R2 = data(ML,1);

% plot(ML, f(ML), '+','linewidth', 2, 'Color', [153, 230, 0]/255, 'MarkerSize', 10);
% hold off ;

%% c寻找 中间的局部最低点
% index = find(Res > 0);
[MinR, LMinR] = min(Res( LMax-windowSize:LMax+windowSize ));
LMinR = LMinR + LMax - windowSize -1;
R3 = data(LMinR, 1);
plot(R3, MinR, '+','linewidth', 2, 'Color', [153, 230, 0]/255, 'MarkerSize', 10);
text(R3, MinR, num2str(R3) );

% [R1, R2, R3]

% imwrite(1, fileName+'.png');
% print(fileName(1:end-4), '-dsvg');

% 寻找数组D中最接近 i 的 n个点
function index = MaxPercent(D, i, n)
    d = sort(abs(D-i));         % 最接近i的几个值
    di = find((D >= -d(n)+i) & (D <= d(n)+i));
    index = di(1:n);            % 返回前n个点
end