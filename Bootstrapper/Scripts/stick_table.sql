CREATE TABLE sticks (
    ticker text NOT NULL primary key,
    buy_levels mediumint[],
    sell_levels mediumint[]
);