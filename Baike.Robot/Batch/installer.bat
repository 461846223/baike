rem --rem --�Զ���װ����ű�
@echo ���ڰ�װ�����������

cd ../
sc create "Baike.Robot" binpath= "%~dp0\Baike.Robot" displayname= "Baike.Robot"
net start "Baike.Robot"

pause


