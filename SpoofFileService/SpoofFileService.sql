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
	"L1" bytea,
	"L2" bytea,
	"CategoryId" smallint not null constraint "FK_FileObject_CategoryId" references "Category"("Id"),
	"ExtensionId" smallint not null constraint "FK_FileObject_ExtensionId" references "Extension"("Id"),
	"IsDeleted" boolean not null default false,
	"Path" text not null,
	"Size" bigint not null,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	check(("L1" is null and "L2" is null and "Size" <= 50000000) or ("L1" is not null and "L2" is not null and "Size" > 50000000)) --50mb
);

create index "IX_FileObject_Fast_Check_L1" on "FileObject"("L1", "Size");
create index "IX_FileObject_Fast_Check_L2" on "FileObject"("L2", "Size");

create or replace function "FindOrCreateFile"(
	fingerprint bytea, 
	l1 bytea, 
	l2 bytea, 
	categoryId smallint, 
	extensionId smallint, 
	filePath text, 
	fileSize bigint)
returns boolean as
$$
declare
	inserted_id bytea;
begin
	INSERT INTO "FileObject"("Id", "L1", "L2", "CategoryId", "ExtensionId", "Path", "Size")
    VALUES (fingerprint, l1, l2, categoryId, extensionId, filePath, fileSize)
    ON CONFLICT ("Id") DO NOTHING
    RETURNING "Id" INTO inserted_id;

    IF inserted_id IS NULL THEN
        RETURN true;
    ELSE
        RETURN false;
    END IF;
end;
$$ language plpgsql;