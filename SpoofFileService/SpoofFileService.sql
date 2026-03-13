create table "Category"
(
	"Id" smallint generated always as identity constraint "PK_Category_Id" primary key,
	"IsDeleted" boolean not null default false,
	"Name" varchar(20) not null constraint "UQ_Category_Name" unique
);

create table "Extension"
(
	"Id" smallint constraint "PK_Extension_Id" primary key,
	"CategoryId" smallint not null constraint "FK_Extension_CategoryId" references "Category"("Id"),
	"IsDeleted" boolean not null default false,
	"Name" varchar(20) not null constraint "UQ_Extension_Name" unique
);

create index "IX_Extension_Name" on "Extension" ("Name") where not "IsDeleted";

create table "FileObject"
(
	"Id" uuid constraint "PK_FileObject_Id" primary key,
	"L1" bytea,
	"L2" bytea,
	"L3" bytea not null,
	"ExtensionId" smallint not null constraint "FK_FileObject_ExtensionId" references "Extension"("Id"),
	"IsDeleted" boolean not null default false,
	"Path" text not null,
	"Size" bigint not null,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP,
	constraint "IX_FileObject_Check_L3" unique("L3", "Size", "ExtensionId"),
	check(("L1" is null and "L2" is null and "Size" <= 50000000) or ("L1" is not null and "L2" is not null and "Size" > 50000000)) --50mb
);

create index "IX_FileObject_Fast_Check_L1" on "FileObject"("L1", "Size", "ExtensionId");
create index "IX_FileObject_Fast_Check_L2" on "FileObject"("L2", "Size", "ExtensionId");


CREATE OR REPLACE FUNCTION "FindOrCreateExtension"(
	extensionId smallint,
	extensionName varchar(20),
	categoryName varchar(20)
)
RETURNS "Extension" AS
$$
DECLARE
    selectionResult "Extension";
	categoryId smallint;
BEGIN
	SELECT * INTO selectionResult FROM "Extension" WHERE "Id" = extensionId LIMIT 1;
	IF selectionResult IS NULL THEN
		SELECT "Id" INTO categoryId FROM "Category" WHERE "Name" = categoryName;
		IF categoryId IS NULL THEN
			INSERT INTO "Category"("Name")
			VALUES (categoryName)
			ON CONFLICT ("Name") DO UPDATE SET "Name" = EXCLUDED."Name"
			RETURNING "Id" INTO categoryId;
		END IF;
		INSERT INTO "Extension"("Id", "Name", "CategoryId")
		VALUES (extensionId, extensionName, categoryId)
		ON CONFLICT ("Name") DO UPDATE SET "Name" = EXCLUDED."Name"
		RETURNING * INTO selectionResult;
	END IF;
	RETURN selectionResult;
END;
$$ language plpgsql;

create or replace function "FindOrCreateFile"(
	id uuid,
	l1 bytea, 
	l2 bytea, 
	l3 bytea, 
	extensionId smallint, 
	filePath text, 
	fileSize bigint)
returns uuid as
$$
declare
	inserted_id uuid;
begin
	INSERT INTO "FileObject"("Id", "L1", "L2", "L3", "ExtensionId", "Path", "Size")
    VALUES (id, l1, l2, l3, extensionId, filePath, fileSize)
    ON CONFLICT ("L3", "Size", "ExtensionId") DO NOTHING
    RETURNING "Id" INTO inserted_id;

    RETURN inserted_id; 
end;
$$ language plpgsql;