-- ================================================
-- 🧱 CHAT APPLICATION DATABASE SCHEMA (PostgreSQL)
-- DDL SCRIPT (TABLES & RELATIONSHIPS)
-- ================================================

CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- ================================================
-- 1️⃣ USERS TABLE
-- ================================================
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "FullName" VARCHAR(150) NOT NULL,
    "UserName" VARCHAR(100) UNIQUE NOT NULL,
    "Password" TEXT NOT NULL,
    "Email" VARCHAR(255) UNIQUE NOT NULL,
    "Phone" VARCHAR(20),
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now()
);

-- ================================================
-- 2️⃣ ACCOUNTS TABLE (Depends on Users)
-- ================================================
CREATE TABLE IF NOT EXISTS "Accounts" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UserId" UUID REFERENCES "Users"("Id") ON DELETE CASCADE,
    "UserName" VARCHAR(100) UNIQUE NOT NULL,
    "Password" TEXT NOT NULL,
    "Email" VARCHAR(255),
    "PhoneNumber" VARCHAR(20),
    "RecoveryEmail" VARCHAR(255),
    "ProfilePicture" TEXT,
    "Bio" TEXT,
    "IsActive" BOOLEAN DEFAULT TRUE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now()
);

-- ================================================
-- 3️⃣ CONTACTS TABLE (Depends on Accounts)
-- ================================================
CREATE TABLE IF NOT EXISTS "Contacts" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "OwnerId" UUID REFERENCES "Accounts"("Id") ON DELETE CASCADE,
    "AccountId" UUID REFERENCES "Accounts"("Id") ON DELETE CASCADE,
    "Alias" VARCHAR(100),
    "AliasPicture" TEXT,
    "Status" VARCHAR(50),
    "LastSeen" TIMESTAMP WITH TIME ZONE,
    "IsBlocked" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    UNIQUE ("OwnerId", "AccountId")
);

-- ================================================
-- 4️⃣ CHATS TABLE (Depends on Contacts)
-- ================================================
CREATE TABLE IF NOT EXISTS "Chats" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "User1Id" UUID REFERENCES "Accounts"("Id") ON DELETE CASCADE,
    "User2Id" UUID REFERENCES "Accounts"("Id") ON DELETE CASCADE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    UNIQUE ("User1Id", "User2Id")
);

-- ================================================
-- 5️⃣ GROUP TABLE (Depends on Accounts)
-- ================================================
CREATE TABLE IF NOT EXISTS "Group" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "OwnerAccountId" UUID REFERENCES "Accounts"("Id") ON DELETE SET NULL,
    "Name" VARCHAR(100) NOT NULL,
    "Description" TEXT,
    "AvatarUrl" TEXT,
    "IsPrivate" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT now()
);

-- ================================================
-- 6️⃣ GROUP MEMBERS TABLE (Depends on Group, Contacts)
-- ================================================
CREATE TABLE IF NOT EXISTS "GroupMembers" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "GroupId" UUID REFERENCES "Group"("Id") ON DELETE CASCADE,
    "AccountId" UUID REFERENCES "Accounts"("Id") ON DELETE CASCADE,
    "Role" VARCHAR(20) DEFAULT 'member',
    "JoinedAt" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    UNIQUE ("GroupId", "AccountId")
);

-- ================================================
-- 7️⃣ MESSAGES TABLE (Depends on Chats, Group, Contacts)
-- ================================================
CREATE TABLE IF NOT EXISTS "Messages" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "SenderId" UUID REFERENCES "Accounts"("Id") ON DELETE SET NULL,
    "ReceiverId" UUID REFERENCES "Accounts"("Id") ON DELETE SET NULL,
    "Type" VARCHAR(20) DEFAULT 'text',
    "Content" TEXT NOT NULL,
    "Time" TIMESTAMP WITH TIME ZONE DEFAULT now(),
    "ChatId" UUID REFERENCES "Chats"("Id") ON DELETE CASCADE,
    "GroupId" UUID REFERENCES "Group"("Id") ON DELETE CASCADE,
    "Status" VARCHAR(20) DEFAULT 'sent',
    "IsEdited" BOOLEAN DEFAULT FALSE,
    "IsDeleted" BOOLEAN DEFAULT FALSE,
    CHECK (
        ("ChatId" IS NOT NULL AND "GroupId" IS NULL)
        OR ("ChatId" IS NULL AND "GroupId" IS NOT NULL)
    )
);

-- ================================================
-- ✅ DDL SCHEMA COMPLETE
-- ================================================
