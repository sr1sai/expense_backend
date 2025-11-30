-- ===========================================
-- Procedures Script for Expense Tracker
-- ===========================================

-- ===========================================
-- Function: insert_user
-- ===========================================
CREATE OR REPLACE FUNCTION public.insert_user(
    p_name TEXT,
    p_email TEXT,
    p_password TEXT,
    p_phone_number TEXT
)
RETURNS INTEGER AS $$
DECLARE 
    new_id INT;
BEGIN
    SET search_path = public, pg_temp;

    INSERT INTO "Users" ("Name", "Email", "Password", "PhoneNumber")
    VALUES (p_name, p_email, p_password, p_phone_number)
    RETURNING "Id" INTO new_id;

    RETURN new_id;
END;
$$ LANGUAGE plpgsql SECURITY DEFINER;


-- ===========================================
-- Function: insert_message
-- ===========================================
CREATE OR REPLACE FUNCTION public.insert_message(
    p_user_id INTEGER,
    p_message_content TEXT
)
RETURNS INTEGER AS $$
DECLARE 
    new_id INT;
BEGIN
    SET search_path = public, pg_temp;

    INSERT INTO "Message" ("UserId", "MessageContent")
    VALUES (p_user_id, p_message_content)
    RETURNING "Id" INTO new_id;

    RETURN new_id;
END;
$$ LANGUAGE plpgsql SECURITY DEFINER;


-- ===========================================
-- Function: insert_payment
-- ===========================================
CREATE OR REPLACE FUNCTION public.insert_payment(
    p_user_id INTEGER,
    p_message_id INTEGER
)
RETURNS INTEGER AS $$
DECLARE 
    new_id INT;
BEGIN
    SET search_path = public, pg_temp;

    INSERT INTO "Payment" ("UserId", "MessageId")
    VALUES (p_user_id, p_message_id)
    RETURNING "Id" INTO new_id;

    RETURN new_id;
END;
$$ LANGUAGE plpgsql SECURITY DEFINER;


-- ===========================================
-- Function: insert_transaction
-- ===========================================
CREATE OR REPLACE FUNCTION public.insert_transaction(
    p_type TEXT,
    p_amount NUMERIC(10,2),
    p_account,
    p_target TEXT,
    p_time TIMESTAMPTZ,
    p_message_id INTEGER
)
RETURNS INTEGER AS $$
DECLARE 
    new_id INT;
BEGIN
    SET search_path = public, pg_temp;

    INSERT INTO "Transaction" (
        "Type",
        "Amount",
        "Account",
        "Target",
        "Time",
        "MessageId"
    )
    VALUES (p_type, p_amount, p_account, p_target, p_time, p_message_id)
    RETURNING "Id" INTO new_id;

    RETURN new_id;
END;
$$ LANGUAGE plpgsql SECURITY DEFINER;

