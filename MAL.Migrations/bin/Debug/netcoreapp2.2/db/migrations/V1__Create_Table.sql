create table if not exists "Users"
(
	"Id" bigserial not null
		constraint "PK_Users"
			primary key,
	"Username" text not null,
	"Password" text not null,
	"Firstname" text,
	"Lastname" text,
	"Token" text
);

insert into "Users" ("Firstname","Lastname","Username","Password", "Token") values ('Jamie','Strange','jstrange','1234', null);