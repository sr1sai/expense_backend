-- ================================================
-- 🧱 CHAT APP DATABASE TRIGGERS (MODIFIED)
-- ✅ Safe duplicate prevention (Option 3)
-- ================================================

-- ================================================
-- FUNCTION: Create self contact when an account is created
-- ================================================
CREATE OR REPLACE FUNCTION create_contact_for_account()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO "Contacts" ("OwnerId", "AccountId", "Alias", "AliasPicture", "Status", "LastSeen")
    SELECT NEW."Id", NEW."Id", 'Myself', NEW."ProfilePicture", 'online', now()
    WHERE NOT EXISTS (
        SELECT 1 FROM "Contacts"
        WHERE "OwnerId" = NEW."Id" AND "AccountId" = NEW."Id"
    );

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- TRIGGER: After inserting into Accounts
DROP TRIGGER IF EXISTS trg_create_contact_for_account ON "Accounts";
CREATE TRIGGER trg_create_contact_for_account
AFTER INSERT ON "Accounts"
FOR EACH ROW
EXECUTE FUNCTION create_contact_for_account();

-- ================================================
-- FUNCTION: Create default account when a user is created
-- ================================================
CREATE OR REPLACE FUNCTION create_account_for_user()
RETURNS TRIGGER AS $$
DECLARE
    new_account_id UUID;
BEGIN
    INSERT INTO "Accounts" (
        "UserId", "UserName", "Password", "Email", "ProfilePicture", "Bio"
    )
    VALUES (
        NEW."Id",
        lower(NEW."UserName"),
        NEW."Password",
        NEW."Email",
        NULL,
        'Welcome to the app!'
    )
    RETURNING "Id" INTO new_account_id;

    -- ✅ Safe self-contact creation (only if not already present)
    INSERT INTO "Contacts" ("OwnerId", "AccountId", "Alias", "Status", "LastSeen")
    SELECT new_account_id, new_account_id, 'Myself', 'online', now()
    WHERE NOT EXISTS (
        SELECT 1 FROM "Contacts"
        WHERE "OwnerId" = new_account_id AND "AccountId" = new_account_id
    );

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- TRIGGER: After inserting into Users
DROP TRIGGER IF EXISTS trg_create_account_for_user ON "Users";
CREATE TRIGGER trg_create_account_for_user
AFTER INSERT ON "Users"
FOR EACH ROW
EXECUTE FUNCTION create_account_for_user();

-- ================================================
-- ✅ Updated triggers complete
-- ================================================
