rem --rem --自动安装服务脚本
@echo 正在安装搜索引擎服务

cd ../
sc create "Weixindq.Robot" binpath= "%~dp0\Weixindq.Robot" displayname= "Weixindq.Robot"
net start "Weixindq.Robot"

pause


