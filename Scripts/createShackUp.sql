use [master] 
go	

if exists(select * from sys.databases where name = 'ShackUp')
drop database ShackUp
go

create database ShackUp
go