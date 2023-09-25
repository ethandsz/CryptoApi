CREATE TABLE sticks (
    ticker text NOT NULL primary key,
    buy_levels integer[],
    sell_levels integer[]
);