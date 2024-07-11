import { requestMap } from "../base";
import { ITodo } from "./types";

const BASE_URL = '/todos'

export const getTodo = (todoId: string): Promise<ITodo> => {
  return requestMap.get<ITodo>(`${BASE_URL}/${todoId}`)
}
