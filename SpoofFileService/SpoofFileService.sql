create table "Extension"
(
	"Id" smallserial constraint "PK_Extension_Id" primary key,
	"IsDeleted" boolean not null default false,
	"Name" varchar(20)
);

create table "Category"
(
	"Id" smallserial constraint "PK_Category_Id" primary key,
	"IsDeleted" boolean not null default false,
	"Name" varchar(20)
);

create table "FileObject"
(
	"Id" bytea not null constraint "PK_FileObject_Id" primary key,
	"PreFingerprint" bigint,
	"CategoryId" smallint not null constraint "FK_FileObject_CategoryId" references "Category"("Id"),
	"ExtensionId" smallint not null constraint "FK_FileObject_ExtensionId" references "Extension"("Id"),
	"IsDeleted" boolean not null default false,
	"Path" text not null,
	"Size" bigint not null,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	check(("PreFingerprint" is null and "Size" <= 50000000) or ("PreFingerprint" is not null and "Size" > 50000000)) --50mb
);

create index "IX_FileObject_Fast_Check" on "FileObject"("PreFingerprint", "Size");