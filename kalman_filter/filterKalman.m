%% 卡尔曼滤波
% 实用参考: 卡尔曼滤波 https://zhuanlan.zhihu.com/p/39912633
% Matlab实现 https://blog.csdn.net/zengxiantao1994/article/details/71170728

filename = ["2020-4-18-1320.csv", "2020-4-18-1324.csv" , "2020-4-18-1339.csv"]; 
address = 'E:\Temp\testdata\';          % 地址
pL = 1000;      % 1khz采样 
sz = [2, 1];    % 保存 k-1估计值 和 k时刻估计值 即可
Q = 1e-10;      % 过程方差，反映连续两个采样点值的方差  Q↑ 稳定性↓ 示数变化速率↑
R = 1e0;        % 测量方差，反映激光器的测量精度    R↑ 稳定性↑ 示数变化速率↓  
% 卡尔曼滤波器 我这里写的是针对线性系统的, 所以对于距离变化这种非线性的情况比较糟糕, 要么反应慢, 要么效果差
% 我们可以设定一个阈值判断, 譬如说前后两秒的均值差距超过 T  , 停用滤波, 差距小于 T (距离不变) 启用卡尔曼滤波

for i = 1  % length(filename)       % 文件名循环    
    data = csvread(strcat(address, filename(i)));
    [row, col] = size(data);                    % 每个csv行列大小
    mean1s = zeros([fix(row/pL), 1]);           % 均值 每 1s 数据
    mean1skal = zeros([fix(row/pL), 1]);        % kalman滤波后均值
    
    %% kalman数据初始化
    dhat = zeros(sz);       % 对距离的后验估计, 即在k时刻，结合当前采样值与k-1时刻先验估计，得到结果
    dhatresult = zeros([pL,1]);     % 存储 dhat 结果
    P = zeros(sz);          % 后验估计方差
    dhatminus = zeros(sz);  % 先验估计值
    Pminus = zeros(sz);     % 先验估计方差 
    K = zeros(sz);          % 卡尔曼增益
    dhat(1) = data(1,1)+0.; % 最初的估计, 这里用均值
    P(1) = 0.1;             % 方差初值

    for j = 1 :  fix(row/pL)  
        % 1s 数据点获取
        d1000 = data((j-1)*pL+1:j*pL, 1);        
        avg = mean(d1000);      % 均值
        mean1s(j) = avg;

        %% 卡尔曼滤波
        for k = 2 : pL
            dhatminus(2) = dhat(1);       % 时间更新, 沿用之前的估计值
            Pminus(2) = P(1) + Q;         % 预测方差=前时刻方差+过程方差
            K(2) = Pminus(2) / (Pminus(2) + R); % 计算卡尔曼增益
            dhat(2) = dhatminus(2) + K(2) * (d1000(k) - dhatminus(2));  % 校正后最优估计结果
            P(2) = (1 - K(2)) * Pminus(2);      % 计算最优估计方差
            
            if k == 2
                dhatresult(k-1) = dhat(k-1);    % 存储 dhat(1)
            end
            dhatresult(k) = dhat(2);
            P(1) = P(2);                       % 更新后验估计方差 
            dhat(1) = dhat(2);                 % 更新下 1s 最初的估计, 这里用上一秒最后一个采样点
        end

        mean1skal(j) = mean(dhatresult(1:end));   % 取 某 点以后的

        % %% 画图 滤波前后 对比结果
        % figure(1);
        % plot((j-1)*pL+1:j*pL, dhat);
        % hold on;
        % % plot([lmax+(j-1)*pL, lmin+(j-1)*pL], [dmax,dmin], 'ro');
        % % hold on;
        % plot((j-1)*pL+1:j*pL, d1000);
        % hold on;
        
        % plot([(j-1)*pL+1, j*pL], [avg, avg], 'g');
        % % legend({'datacurve',strcat('extreme point \delta:', num2str(delta), ' avg:', num2str(avg)) }, 'Location', 'best');
        % hold off;

        % % 保存图片
        % saveas(1, strcat(address, 'figure\kalman',  filename(i), '-',num2str(j), '.jpg'));           
        % close;
    end
    figure(i);

    plot(mean1skal(1:end), '-o');
    hold on;
    
    title('kalman vs ordinary data mean');
    xlabel('s');
    ylabel('value');
    
    plot(mean1s(1:end), '--^');
    hold off;   
end

