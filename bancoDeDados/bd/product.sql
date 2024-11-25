

create database crud;

use crud;

create table product(
	id int primary key auto_increment,
	name varchar(255),
	price decimal(10,2)
);


insert into product (name, price) values
					('Notebook', 1000.00),
					('Mouse', 100.00),
					('Teclado', 100.00),
					('Monitor', 1000.00),
					('Mousepad', 100.00);