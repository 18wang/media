% 第一列 每一千个数画一个曲线, 算极差值 噪点 滤除

filename = ["2020-4-18-1320.csv", "2020-4-18-1324.csv" , "2020-4-18-1339.csv"]; 
address = 'E:\Temp\testdata\';          % 地址
pL = 1000;

for i = 1 : length(filename)
    data = csvread(strcat(address, filename(i)));
    [row, col] = size(data);                    % 每个csv行列大小
    j = 1;
    for j = 1 : 2 % fix(row/1000)   
        d1000 = data((j-1)*pL+1:j*pL, 1);
        [dmax, lmax] = max(d1000);  [dmin, lmin] = min(d1000);
        delta = dmax - dmin;
        figure(1);
        plot((j-1)*pL+1:j*pL, d1000);
        hold on;
        plot([lmax+(j-1)*pL, lmin+(j-1)*pL], [dmax,dmin], 'ro');
        hold on;
        avg = mean(d1000);
        plot([(j-1)*pL+1, j*pL], [avg, avg], 'g');
        legend({'datacurve',strcat('extreme point \delta:', num2str(delta), ' avg:', num2str(avg)) }, 'Location', 'best');
        hold off;
         %保存图片
        saveas(1, strcat(address, 'figure\',  filename(i), '-',num2str(j), '.jpg'));           
        close;
    end
end

