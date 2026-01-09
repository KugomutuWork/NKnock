const baseUrl = import.meta.env.VITE_API_BASE_URL;

export type Todo = {
  id: number;
  title: string;
  isDone: boolean;
};

export async function health(): Promise<string> {
  const res = await fetch(`${baseUrl}/health`);
  if (!res.ok) throw new Error("Health check failed");
  return res.text();
}

export async function getTodos(): Promise<Todo[]> {
  const res = await fetch(`${baseUrl}/api/todos`);
  if (!res.ok) throw new Error("Failed to fetch todos");
  return res.json();
}

export async function createTodo(title: string): Promise<Todo> {
  const res = await fetch(`${baseUrl}/api/todos`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ title }),
  });
  if (!res.ok) throw new Error("Failed to create todo");
  return res.json();
}

export async function setDone(id: number, isDone: boolean): Promise<void> {
  const res = await fetch(`${baseUrl}/api/todos/${id}/done`, {
    method: "PATCH",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ isDone }),
  });
  if (!res.ok) throw new Error("Failed to update todo");
}

export async function deleteTodo(id: number): Promise<void> {
  const res = await fetch(`${baseUrl}/api/todos/${id}`, { method: "DELETE" });
  if (!res.ok) throw new Error("Failed to delete todo");
}
