rem --rem --自动安装服务脚本
@echo 正在安装搜索引擎服务

cd ../
sc create "Baike.Robot" binpath= "%~dp0\Baike.Robot" displayname= "Baike.Robot"
net start "Baike.Robot"

pause


