sp_dynamicDT

elseif \_mode = 'SeniorOSCA' then
SET @sql = "SELECT
dersenior.OldRefID,
dersenior.bDate,
dersenior.sex,
dersenior.locationID,
dersenior.brgyID,
dersenior.ncscrrn,
dersenior.DSWDPensioner,
dersenior.status,
dersenior.age,
dersenior.fullName,
dersenior.fNameOSCA,
dersenior.fNameCPS,
dersenior.RefID,
dersenior.createDate,
dersenior.createdDate
FROM (
SELECT
osc.OldRefID,

        DATE_FORMAT(
            CASE WHEN m.ID IS NOT NULL THEN m.bdate ELSE osc.bDate END,
            '%M %d, %Y'
        ) AS bDate,
        CASE WHEN m.ID IS NOT NULL THEN m.sex ELSE osc.sex END AS sex,
        CASE WHEN m.ID IS NOT NULL THEN m.fName ELSE osc.fName END AS fName,
        CASE WHEN m.ID IS NOT NULL THEN m.mName ELSE osc.mName END AS mName,
        CASE WHEN m.ID IS NOT NULL THEN m.lName ELSE osc.lName END AS lName,
        CASE WHEN m.ID IS NOT NULL THEN m.suffix ELSE osc.suffix END AS suffix,
        CASE
            WHEN m.ID IS NOT NULL THEN
                 m.locationID
        END AS locationID,
        CASE
            WHEN m.ID IS NOT NULL THEN
                 m.brgyID
        END AS brgyID,
        CASE WHEN m.ID IS NOT NULL THEN m.ncscrrn ELSE osc.ncscrrn END AS ncscrrn,
        CASE WHEN m.ID IS NOT NULL THEN m.DSWDPensioner ELSE osc.DSWDpensioner END AS DSWDPensioner,
        CASE WHEN m.ID IS NOT NULL THEN m.status ELSE osc.status END AS status,
        TIMESTAMPDIFF(YEAR, COALESCE(m.bdate, osc.bDate), CURDATE()) AS age,
        CONCAT(
            CASE WHEN m.ID IS NOT NULL THEN m.lName ELSE osc.lName END,
            CASE
                WHEN (m.ID IS NOT NULL AND m.suffix IS NOT NULL AND m.suffix != '') THEN CONCAT(' ', m.suffix)
                WHEN (osc.suffix IS NOT NULL AND osc.suffix != '') THEN CONCAT(' ', osc.suffix)
                ELSE ''
            END,
            ', ',
            CASE WHEN m.ID IS NOT NULL THEN m.fName ELSE osc.fName END,
            ' ',
            CASE WHEN m.ID IS NOT NULL THEN m.mName ELSE osc.mName END
        ) AS fullName,
        osc.fName AS fNameOSCA,
        m.fName AS fNameCPS,
        osc.RefID,
        DATE_FORMAT(m.createDate, '%Y-%m-%d') AS createDate,
        DATE_FORMAT(osc.createDate, '%Y-%m-%d') AS createdDate
    FROM
        egov.osca_seniorcitizen_tbl AS osc
        LEFT JOIN egov.maincitizenprofile_tbl AS m ON m.RefID = osc.OldRefID

) AS dersenior
";
