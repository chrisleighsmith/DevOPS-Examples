CREATE STREAM cc_authorizations( \
    credit_card_nbr STRING, \
    auth_time BIGINT, \
    status STRING) \
    WITH(KAFKA_TOPIC='cc-authorization-topic', VALUE_FORMAT='AVRO', KEY='credit_card_nbr');

CREATE TABLE CC_POTENTIAL_FRAUD AS \
    SELECT credit_card_nbr, COUNT(*) attempts \
    FROM cc_authorizations \
    WINDOW TUMBLING (SIZE 3 SECONDS) \
    WHERE status='FAILED' \
    GROUP BY credit_card_nbr \
    HAVING COUNT(*)>=3;

CREATE STREAM CC_POTENTIAL_FRAUD_STREAM (\
    credit_card_nbr string, attempts bigint) \
    WITH (kafka_topic='CC_POTENTIAL_FRAUD', value_format='AVRO');

CREATE STREAM CC_POTENTIAL_FRAUD_COUNTS \
    WITH (PARTITIONS=3,REPLICAS=1) AS \
    SELECT * FROM CC_POTENTIAL_FRAUD_STREAM WHERE ROWTIME IS NOT NULL;

