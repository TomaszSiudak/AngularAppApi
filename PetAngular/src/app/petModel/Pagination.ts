export interface Pagination {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

export class Pages<T> {
    result: T;
    pagination: Pagination;
}
