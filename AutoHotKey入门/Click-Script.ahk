+s::            ; PDF 编辑模式, 单页显示模式
Loop, 1         ; 循环次数 = 页数
{
    Send {Click, right}     ; 右击
    Send {Delete}           ; 删除
    Send {Esc}              ; 退出(如需要)
    Send {Down}             ; 下一页
}
return