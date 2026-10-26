import { ErrorResponse } from "./ErrorResponse";
import { PaginatorResponse } from "./PaginatorResponse";

export interface ApiResponse<T> {
  success: boolean;
  payload: T;
  error: ErrorResponse;
  paginator: PaginatorResponse;
  errors: { [key: string]: string[] };
}
