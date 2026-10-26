import { ErrorResponse } from "./ErrorResponse";

export interface FileResponse<Blob> {
    success: boolean;
    payload: Blob;
    error: ErrorResponse;
}
