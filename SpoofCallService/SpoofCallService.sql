CREATE table "User"
(
    "Id" UUID CONSTRAINT "PK_User_Id" PRIMARY KEY DEFAULT uuidv7(),
	"Name" VARCHAR(100) NOT NULL,
	"Login" VARCHAR(100) NOT NULL unique,
	"IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    "LastModified" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE table "Chat"
(
    "Id" UUID CONSTRAINT "PK_Chat_Id" PRIMARY KEY DEFAULT uuidv7(),
	"OwnerId" UUID CONSTRAINT "FK_Chat_OwnerId" REFERENCES "User"("Id") ON DELETE SET NULL,
    "UniqueName" VARCHAR(100) unique NOT NULL,
    "Name" VARCHAR(100) NOT NULL,
	"IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    "LastModified" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE table "ChatUser"
(
    "ChatId" UUID NOT NULL CONSTRAINT "FK_ChatUser_ChatId" REFERENCES "Chat"("Id") ON DELETE CASCADE,
    "UserId" UUID NOT NULL CONSTRAINT "FK_ChatUser_UserId" REFERENCES "User"("Id") ON DELETE CASCADE,
	"Rules" BIGINT DEFAULT 0 NOT NULL,
	"IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    "LastModified" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "PK_ChatUser_Id" PRIMARY KEY("ChatId", "UserId")
);

CREATE TYPE "TopologyType" AS ENUM 
(
    'Mesh',
	'SFU'
);

CREATE TYPE "EndReasonType" AS ENUM 
(
    'Hangup',
	'Timeout',
	'AllLeft',
	'Error'
);

CREATE TYPE "CallStatusType" AS ENUM 
(
    'Ringing',
	'Active',
	'Ended',
	'Cancelled'
);

CREATE TYPE "CallMemberStatusType" AS ENUM 
(
    'Invited',
	'Joined',
	'Left',
	'Missed',
	'Declined'
);

CREATE TYPE "SessionEndReasonType" AS ENUM
(
    'Hangup',
    'Disconnected',
    'Kicked',
    'Error'
);

CREATE table "Call"
(
    "Id" UUID CONSTRAINT "PK_Call_Id" PRIMARY KEY DEFAULT uuidv7(),
    "ChatId" UUID NOT NULL CONSTRAINT "FK_Call_ChatId" REFERENCES "Chat"("Id") ON DELETE CASCADE,
    "InitiatorId" UUID NOT NULL CONSTRAINT "FK_Call_InitiatorId" REFERENCES "User"("Id") ON DELETE CASCADE,
    "Topology" "TopologyType" NOT NULL,
    "CallStatus" "CallStatusType" NOT NULL,
    "EndReason" "EndReasonType",
    "StartedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "AnsweredAt" TIMESTAMPTZ,
    "EndedAt" TIMESTAMPTZ,
	"IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    "LastModified" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "RoomId" TEXT,
    CHECK ("AnsweredAt" IS NULL OR ("AnsweredAt" >= "StartedAt" AND ("EndedAt" IS NULL OR "EndedAt" >= "AnsweredAt"))),
    CHECK ("EndedAt" IS NULL OR "EndedAt" >= "StartedAt")
);

CREATE table "CallMember"
(
    "CallId" UUID NOT NULL CONSTRAINT "FK_CallMember_CallId" REFERENCES "Call"("Id") ON DELETE CASCADE,
    "UserId" UUID NOT NULL CONSTRAINT "FK_CallMember_UserId" REFERENCES "User"("Id") ON DELETE CASCADE,
    "CallMemberStatus" "CallMemberStatusType" NOT NULL,
	"IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    "LastModified" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "PK_CallMember_Id" PRIMARY KEY("CallId", "UserId")
);

CREATE table "CallMemberSession"
(
    "Id" UUID CONSTRAINT "PK_CallMemberSession_Id" PRIMARY KEY DEFAULT uuidv7(),
    "CallId" UUID NOT NULL,
    "UserId" UUID NOT NULL,
    "EndReason" "SessionEndReasonType",
    "JoinedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "LeftAt" TIMESTAMPTZ,
    CONSTRAINT "FK_CallMemberSession_CallMemberId" FOREIGN KEY("CallId", "UserId") REFERENCES "CallMember"("CallId", "UserId") ON DELETE CASCADE,
    CHECK ("LeftAt" IS NULL OR "LeftAt" >= "JoinedAt"),
    CHECK (("LeftAt" IS NULL AND "EndReason" IS NULL) OR ("LeftAt" IS NOT NULL AND "EndReason" IS NOT NULL))
);

CREATE INDEX "IX_CallMemberSession_Opened" ON "CallMemberSession"("CallId", "UserId") WHERE "LeftAt" IS NULL;

CREATE INDEX "IX_CallMemberSession_Recent" ON "CallMemberSession"("UserId", "JoinedAt" DESC);