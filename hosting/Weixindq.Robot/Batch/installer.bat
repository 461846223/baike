rem --rem --�Զ���װ����ű�
@echo ���ڰ�װ�����������

cd ../
sc create "Weixindq.Robot" binpath= "%~dp0\Weixindq.Robot" displayname= "Weixindq.Robot"
net start "Weixindq.Robot"

pause


