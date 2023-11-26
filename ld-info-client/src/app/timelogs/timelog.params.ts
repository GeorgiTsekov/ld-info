export class TimelogParams {
    public fromDate?: string;
    public toDate?: string;
    public sortBy?: string;
    public isAscending?: boolean;
    public pageNumber?: number;
    public pageSize?: number;

    constructor(
        fromDate?: string,
        toDate?: string,
        sortBy?: string,
        isAscending?: boolean,
        pageNumber?: number,
        pageSize?: number) {
        this.fromDate = fromDate;
        this.toDate = toDate;
        this.sortBy = sortBy;
        this.isAscending = isAscending;
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
}
    }