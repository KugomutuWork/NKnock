import { useEffect, useState } from "react";
import {
  createTodo,
  deleteTodo,
  getTodos,
  health,
  setDone,
  type Todo,
} from "./lib/api";

export default function App() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [title, setTitle] = useState("");
  const [loading, setLoading] = useState(false);
  const [err, setErr] = useState<string | null>(null);
  const [healthText, setHealthText] = useState<string>("");

  async function reload() {
    const data = await getTodos();
    setTodos(data);
  }

  useEffect(() => {
    (async () => {
      try {
        setErr(null);
        setHealthText(await health());
        await reload();
      } catch (e) {
        setErr(e instanceof Error ? e.message : "Unknown error");
      }
    })();
  }, []);

  async function onAdd() {
    if (!title.trim()) return;
    setLoading(true);
    setErr(null);
    try {
      await createTodo(title.trim());
      setTitle("");
      await reload();
    } catch (e) {
      setErr(e instanceof Error ? e.message : "Unknown error");
    } finally {
      setLoading(false);
    }
  }

  async function onToggle(todo: Todo) {
    setLoading(true);
    setErr(null);
    try {
      await setDone(todo.id, !todo.isDone);
      await reload();
    } catch (e) {
      setErr(e instanceof Error ? e.message : "Unknown error");
    } finally {
      setLoading(false);
    }
  }

  async function onDelete(id: number) {
    setLoading(true);
    setErr(null);
    try {
      await deleteTodo(id);
      await reload();
    } catch (e) {
      setErr(e instanceof Error ? e.message : "Unknown error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div
      style={{ maxWidth: 720, margin: "40px auto", fontFamily: "system-ui" }}
    >
      <h1>Todo</h1>
      <p style={{ opacity: 0.7 }}>Health: {healthText || "-"}</p>

      <div style={{ display: "flex", gap: 8 }}>
        <input
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="New todo..."
          style={{ flex: 1, padding: 8 }}
        />
        <button onClick={onAdd} disabled={loading}>
          Add
        </button>
      </div>

      {err && <p style={{ color: "crimson" }}>{err}</p>}

      <ul style={{ paddingLeft: 16 }}>
        {todos.map((t) => (
          <li
            key={t.id}
            style={{ display: "flex", gap: 8, alignItems: "center" }}
          >
            <label
              style={{ flex: 1, display: "flex", gap: 8, alignItems: "center" }}
            >
              <input
                type="checkbox"
                checked={t.isDone}
                onChange={() => onToggle(t)}
                disabled={loading}
              />
              <span
                style={{ textDecoration: t.isDone ? "line-through" : "none" }}
              >
                {t.title}
              </span>
            </label>
            <button onClick={() => onDelete(t.id)} disabled={loading}>
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}
