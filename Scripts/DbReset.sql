use [ShackUp]
go

if exists(select * from INFORMATION_SCHEMA.ROUTINES
where ROUTINE_NAME = 'DbReset')
drop procedure DbReset
go

create procedure DbReset as
begin

delete from Contacts
delete from Favorites
delete from Listings
delete from States
delete from BathroomTypes
delete from AspNetUsers where Id in ('00000000-0000-0000-0000-000000000000', '11111111-1111-1111-1111-111111111111')

dbcc checkident ('Listings', reseed, 1)

insert into States (StateId, StateName)
values ('OH', 'Ohio'),
       ('KY', 'Kentucky'),
	   ('MN', 'Minnesota')	

set identity_insert BathroomTypes on

insert into BathroomTypes (BathroomTypeId, BathroomTypename)
values (1, 'Indoor'),
	   (2, 'Outdoor'),
	   (3, 'None')

set identity_insert BathroomTypes off

insert into AspNetUsers (id, EmailConfirmed, PhoneNumberConfirmed, Email, StateId, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName)
values('00000000-0000-0000-0000-000000000000', 0, 0, 'test@test.com', 'OH', 0, 0, 0, 'test')

insert into AspNetUsers (id, EmailConfirmed, PhoneNumberConfirmed, Email, StateId, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName)
values('11111111-1111-1111-1111-111111111111', 0, 0, 'test2@test.com', 'OH', 0, 0, 0, 'test2')

set identity_insert Listings on

insert into Listings(ListingId, UserId, StateId, BathroomTypeId, Nickname, City, Rate, 
					 SquareFootage, HasElectric, HasHeat, ImageFileName, ListingDescription)
values(1, '00000000-0000-0000-0000-000000000000', 'OH', 1, 'Test shack 1', 'Cleveland', 100, 400, 0, 1, 'placeholder.png', 'Description 1'),
(2, '00000000-0000-0000-0000-000000000000', 'OH', 1, 'Test shack 2', 'Cleveland', 110, 410, 0, 1, 'placeholder.png', null),
(3, '00000000-0000-0000-0000-000000000000', 'OH', 1, 'Test shack 3', 'Cleveland', 120, 420, 0, 1, 'placeholder.png', null),
(4, '00000000-0000-0000-0000-000000000000', 'OH', 1, 'Test shack 4', 'Cleveland', 130, 430, 0, 1, 'placeholder.png', 'Description 4'),
(5, '00000000-0000-0000-0000-000000000000', 'OH', 1, 'Test shack 5', 'Cleveland', 140, 440, 0, 1, 'placeholder.png', null),
(6, '00000000-0000-0000-0000-000000000000', 'OH', 1, 'Test shack 6', 'Cleveland', 150, 450, 0, 1, 'placeholder.png', null)

set identity_insert Listings off

insert into Favorites (ListingId, UserId)
Values(1, '11111111-1111-1111-1111-111111111111'),
(2, '11111111-1111-1111-1111-111111111111')

insert into Contacts (ListingId, UserId)
Values(1, '11111111-1111-1111-1111-111111111111'),
(3, '11111111-1111-1111-1111-111111111111')

end
go