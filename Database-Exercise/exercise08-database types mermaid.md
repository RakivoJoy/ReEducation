```mermaid
flowchart TD
    Start([What is your primary workload?]) --> Q1{Need strict ACID &<br/>complex joins?}
    
    Q1 -- Yes --> Q2{Scale beyond<br/>single-node writes?}
    Q2 -- No --> SQL[<b>Traditional SQL</b><br/>PostgreSQL / MySQL<br/><i>Billing, Auth, Core SaaS</i>]
    Q2 -- Yes --> DistSQL[<b>Distributed / Sharded SQL</b><br/>CockroachDB / Vitess<br/><i>Global Finance, Multi-tenant SaaS</i>]
    
    Q1 -- No --> Q3{What is the dominant<br/>data shape & speed?}
    
    Q3 -- Sub-ms lookups / cache --> KV[<b>Key-Value Store</b><br/>Redis / DynamoDB<br/><i>Sessions, Carts, Rate Limits</i>]
    Q3 -- Nested JSON / evolving schema --> Doc[<b>Document Store</b><br/>MongoDB / Firestore<br/><i>Catalogs, CMS, Profiles</i>]
    Q3 -- Massive append-only writes --> Wide[<b>Wide-Column Store</b><br/>Cassandra / ScyllaDB<br/><i>GPS Pings, Logs, Chat</i>]
    Q3 -- Deep relationships / AI vectors --> Spec[<b>Graph or Vector DB</b><br/>Neo4j / pgvector<br/><i>Recommendations, Fraud, RAG</i>]
