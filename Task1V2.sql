/*  
1.	Написать SQL-запрос, который возвращает объем продаж в количественном выражении в разрезе сотрудников за период с 01.10.2013 по 07.10.2013:
●	Фамилия и имя сотрудника;
●	Объем продаж сотрудника.
*/  
SELECT CONCAT(seller.Surname, seller.Name) AS "ФИО", SUM (sales.Quantity) AS "Количество"
FROM SALES AS sales, SELLERS AS seller
   WHERE sales.IDSEL = seller.ID AND sales.Date
 BETWEEN '20131001' AND '20131007'
      GROUP BY seller.Surname, seller.Name
	
/*  
2.	На основании созданного в первом задании запроса написать SQL-запрос, который возвращает процент объема продаж в разрезе сотрудников и продукции за период с 01.10.2013 по 07.10.2013:
●	Наименование продукции;
●	Фамилия и имя сотрудника;
●	Процент продаж сотрудником данного вида продукции (продажи сотрудника данной продукции/общее число продаж данной продукции).
*/  	
SELECT sellers.Name, sellers.Surname, products.Name, (100. * SUM(sales.Quantity)) / SUM(SUM(sales.Quantity)) OVER (partition by products.Name)
FROM
 Sales AS sales join
 Sellers AS sellers ON sellers.ID = sales.IDSel join
 Products AS products ON products.ID = sales.IDProd
WHERE
 sales.Date BETWEEN '20131001' AND '20131007'
GROUP BY sellers.Name, sellers.Surname, products.Name;