CREATE OR REPLACE FUNCTION public.get_client_daily_payments(
	p_client_id bigint,
	p_start_date date,
	p_end_date date)
    RETURNS TABLE(dt date, amount numeric) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
BEGIN
    RETURN QUERY
    WITH 
    date_range AS (
        SELECT generate_series(
            p_start_date, 
            p_end_date, 
            '1 day'::interval
        )::date AS dt
    ),
    client_daily_payments AS (
        SELECT 
            cp.dt::date AS payment_date,
            SUM(cp.amount)::numeric AS total_amount
        FROM client_payments cp
        WHERE cp.client_id = p_client_id
            AND cp.dt::date BETWEEN p_start_date AND p_end_date
        GROUP BY cp.dt::date
    )
    SELECT 
        dr.dt,
        COALESCE(cdp.total_amount, 0) AS amount
    FROM date_range dr
    LEFT JOIN client_daily_payments cdp ON dr.dt = cdp.payment_date
    ORDER BY dr.dt;
END;
$BODY$;

ALTER FUNCTION public.get_client_daily_payments(bigint, date, date)
    OWNER TO postgres;
