create table "FileObject"
(
	"Id" uuid not null constraint "PK_FileObject_Id" primary key default uuidv7(),
	"IsDeleted" boolean not null default false,
	"FilePath" text not null,
	"LastModified" timestamp not null default CURRENT_TIMESTAMP
);