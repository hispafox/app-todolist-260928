import { useEffect, useState, type FormEvent } from "react";
import {
  Check,
  Circle,
  ClipboardList,
  ListFilter,
  Pencil,
  Plus,
  RotateCcw,
  Trash2,
  X,
} from "lucide-react";
import packageJson from "../package.json";
import { api } from "./api";
import type { FiltroTareas, PrioridadTarea, Tarea, Usuario } from "./types";

const filtros: FiltroTareas[] = ["Todas", "Pendiente", "Completada"];
const versionApp = packageJson.version;

function App() {
  const [tareas, setTareas] = useState<Tarea[]>([]);
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [filtro, setFiltro] = useState<FiltroTareas>("Todas");
  const [titulo, setTitulo] = useState("");
  const [prioridad, setPrioridad] = useState<PrioridadTarea>("Media");
  const [responsableId, setResponsableId] = useState("");
  const [tareaEditando, setTareaEditando] = useState<number | null>(null);
  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let activo = true;
    Promise.all([api.listarTareas(), api.listarUsuarios()])
      .then(([listaTareas, listaUsuarios]) => {
        if (activo) {
          setTareas(listaTareas);
          setUsuarios(listaUsuarios);
        }
      })
      .catch((fallo: unknown) => {
        if (activo) {
          setError(fallo instanceof Error ? fallo.message : "No se pudo cargar la lista.");
        }
      })
      .finally(() => {
        if (activo) setCargando(false);
      });

    return () => {
      activo = false;
    };
  }, []);

  const tareasVisibles = filtro === "Todas" ? tareas : tareas.filter((tarea) => tarea.estado === filtro);
  const pendientes = tareas.filter((tarea) => tarea.estado === "Pendiente").length;
  const completadas = tareas.length - pendientes;

  async function enviarFormulario(evento: FormEvent<HTMLFormElement>) {
    evento.preventDefault();
    setGuardando(true);
    setError(null);
    const solicitud = {
      titulo: titulo.trim(),
      prioridad,
      responsableId: responsableId ? Number(responsableId) : null,
    };

    try {
      const tarea = tareaEditando === null
        ? await api.crearTarea(solicitud)
        : await api.editarTarea(tareaEditando, solicitud);
      setTareas((actuales) => tareaEditando === null
        ? [...actuales, tarea]
        : actuales.map((actual) => actual.id === tarea.id ? tarea : actual));
      limpiarFormulario();
    } catch (fallo) {
      setError(fallo instanceof Error ? fallo.message : "No se pudo guardar la tarea.");
    } finally {
      setGuardando(false);
    }
  }

  function iniciarEdicion(tarea: Tarea) {
    setTareaEditando(tarea.id);
    setTitulo(tarea.titulo);
    setPrioridad(tarea.prioridad);
    setResponsableId(tarea.responsableId?.toString() ?? "");
    setError(null);
    document.getElementById("titulo-tarea")?.focus();
  }

  function limpiarFormulario() {
    setTareaEditando(null);
    setTitulo("");
    setPrioridad("Media");
    setResponsableId("");
  }

  async function alternarEstado(tarea: Tarea) {
    const estado = tarea.estado === "Pendiente" ? "Completada" : "Pendiente";
    setError(null);
    try {
      const actualizada = await api.cambiarEstado(tarea.id, estado);
      setTareas((actuales) => actuales.map((actual) => actual.id === tarea.id ? actualizada : actual));
    } catch (fallo) {
      setError(fallo instanceof Error ? fallo.message : "No se pudo cambiar el estado.");
    }
  }

  async function eliminarTarea(tarea: Tarea) {
    if (!window.confirm(`¿Eliminar «${tarea.titulo}»?`)) return;
    setError(null);
    try {
      await api.eliminarTarea(tarea.id);
      setTareas((actuales) => actuales.filter((actual) => actual.id !== tarea.id));
      if (tareaEditando === tarea.id) limpiarFormulario();
    } catch (fallo) {
      setError(fallo instanceof Error ? fallo.message : "No se pudo eliminar la tarea.");
    }
  }

  return (
    <main className="page-shell">
      <header className="topbar">
        <a className="brand" href="#inicio" aria-label="Lista de tareas, inicio">
          <span className="brand-mark"><Check size={19} strokeWidth={3} /></span>
          <span>hecho<span className="brand-period">.</span></span>
        </a>
        <span className="topbar-note">Tu espacio de trabajo</span>
      </header>

      <section className="workspace" id="inicio">
        <div className="page-heading">
          <div>
            <p className="eyebrow">TABLERO PERSONAL</p>
            <h1>Mis tareas</h1>
            <p className="page-subtitle">Un paso a la vez. Lo importante, en orden.</p>
          </div>
          <div className="day-stamp" aria-label="Lista local">
            <span className="day-stamp-dot" />
            <span>LISTA LOCAL</span>
          </div>
        </div>

        <section className="summary-strip" aria-label="Resumen de tareas">
          <div className="summary-item">
            <span className="summary-value">{tareas.length.toString().padStart(2, "0")}</span>
            <span className="summary-label">en total</span>
          </div>
          <div className="summary-divider" />
          <div className="summary-item">
            <span className="summary-value summary-value-active">{pendientes.toString().padStart(2, "0")}</span>
            <span className="summary-label">por hacer</span>
          </div>
          <div className="summary-divider" />
          <div className="summary-item">
            <span className="summary-value summary-value-done">{completadas.toString().padStart(2, "0")}</span>
            <span className="summary-label">completadas</span>
          </div>
          <div className="summary-progress" aria-hidden="true">
            <div className="summary-progress-fill" style={{ width: tareas.length ? `${(completadas / tareas.length) * 100}%` : "0%" }} />
          </div>
        </section>

        <div className="content-grid">
          <section className="list-section" aria-labelledby="list-title">
            <div className="section-toolbar">
              <div className="section-title-wrap">
                <ClipboardList size={20} strokeWidth={1.8} aria-hidden="true" />
                <h2 id="list-title">Tu lista</h2>
                <span className="task-count">{tareasVisibles.length}</span>
              </div>
              <div className="filter-control" aria-label="Filtrar tareas">
                <ListFilter size={16} aria-hidden="true" />
                {filtros.map((opcion) => (
                  <button
                    className={filtro === opcion ? "filter-button is-selected" : "filter-button"}
                    key={opcion}
                    onClick={() => setFiltro(opcion)}
                    type="button"
                  >
                    {opcion === "Pendiente" ? "Pendientes" : opcion === "Completada" ? "Completadas" : opcion}
                  </button>
                ))}
              </div>
            </div>

            {error && <p className="error-message" role="alert">{error}</p>}

            {cargando ? (
              <div className="loading-state" role="status">Cargando tareas…</div>
            ) : tareasVisibles.length === 0 ? (
              <div className="empty-state">
                <span className="empty-icon"><ClipboardList size={25} strokeWidth={1.6} /></span>
                <h3>{tareas.length === 0 ? "Tu lista empieza aquí" : "No hay tareas en esta vista"}</h3>
                <p>{tareas.length === 0 ? "Anota lo que tienes en mente y dale forma a tu día." : "Prueba otro filtro o añade una tarea nueva."}</p>
              </div>
            ) : (
              <ul className="task-list">
                {tareasVisibles.map((tarea) => (
                  <li className={tarea.estado === "Completada" ? "task-row is-complete" : "task-row"} key={tarea.id}>
                    <button
                      aria-label={tarea.estado === "Pendiente" ? `Completar ${tarea.titulo}` : `Reabrir ${tarea.titulo}`}
                      className="status-button"
                      onClick={() => void alternarEstado(tarea)}
                      title={tarea.estado === "Pendiente" ? "Marcar como completada" : "Reabrir tarea"}
                      type="button"
                    >
                      {tarea.estado === "Completada" ? <Check size={15} strokeWidth={2.8} /> : <Circle size={19} strokeWidth={1.7} />}
                    </button>
                    <div className="task-main">
                      <span className="task-title">{tarea.titulo}</span>
                      <div className="task-meta">
                        <span className={`priority-label priority-${tarea.prioridad.toLowerCase()}`}>
                          <span className="priority-dot" />{tarea.prioridad}
                        </span>
                        {tarea.responsable && <span className="assignee-label">{tarea.responsable}</span>}
                      </div>
                    </div>
                    <div className="task-actions">
                      <button aria-label={`Editar ${tarea.titulo}`} className="icon-button" onClick={() => iniciarEdicion(tarea)} title="Editar" type="button">
                        <Pencil size={16} />
                      </button>
                      <button aria-label={`Eliminar ${tarea.titulo}`} className="icon-button icon-button-danger" onClick={() => void eliminarTarea(tarea)} title="Eliminar" type="button">
                        <Trash2 size={16} />
                      </button>
                    </div>
                  </li>
                ))}
              </ul>
            )}
          </section>

          <aside className="form-section" aria-labelledby="form-title">
            <div className="form-heading">
              <span className="form-icon">{tareaEditando === null ? <Plus size={17} /> : <Pencil size={16} />}</span>
              <div>
                <p className="eyebrow">{tareaEditando === null ? "NUEVA NOTA" : "ACTUALIZAR"}</p>
                <h2 id="form-title">{tareaEditando === null ? "Añadir tarea" : "Editar tarea"}</h2>
              </div>
            </div>
            <form className="task-form" onSubmit={(evento) => void enviarFormulario(evento)}>
              <label className="field-label" htmlFor="titulo-tarea">¿Qué tienes que hacer?</label>
              <input
                autoComplete="off"
                id="titulo-tarea"
                maxLength={200}
                onChange={(evento) => setTitulo(evento.target.value)}
                placeholder="Escribe una tarea…"
                required
                value={titulo}
              />
              <div className="form-fields-row">
                <div className="form-field">
                  <label className="field-label" htmlFor="prioridad-tarea">Prioridad</label>
                  <select id="prioridad-tarea" onChange={(evento) => setPrioridad(evento.target.value as PrioridadTarea)} value={prioridad}>
                    <option value="Baja">Baja</option>
                    <option value="Media">Media</option>
                    <option value="Alta">Alta</option>
                  </select>
                </div>
                <div className="form-field">
                  <label className="field-label" htmlFor="responsable-tarea">Responsable</label>
                  <select id="responsable-tarea" onChange={(evento) => setResponsableId(evento.target.value)} value={responsableId}>
                    <option value="">Sin asignar</option>
                    {usuarios.map((usuario) => <option key={usuario.id} value={usuario.id}>{usuario.nombre}</option>)}
                  </select>
                </div>
              </div>
              <div className="form-actions">
                {tareaEditando !== null && (
                  <button className="cancel-button" onClick={limpiarFormulario} type="button">
                    <X size={16} /> Cancelar
                  </button>
                )}
                <button className="submit-button" disabled={guardando || !titulo.trim()} type="submit">
                  {tareaEditando === null ? <Plus size={17} /> : <Check size={17} />}
                  {guardando ? "Guardando…" : tareaEditando === null ? "Añadir tarea" : "Guardar cambios"}
                </button>
              </div>
            </form>
            <div className="form-footnote"><RotateCcw size={13} /> Al guardar, los cambios quedan en SQLite.</div>
          </aside>
        </div>
        <footer className="page-footer">
          <span>hecho. <span className="footer-divider">/</span> Una lista sencilla para avanzar.</span>
          <span className="page-footer-meta">
            <span className="page-version" aria-label={`Versión de la aplicación ${versionApp}`}>v{versionApp}</span>
            <span className="footer-divider">/</span>
            <span>DATOS LOCALES · SQLITE</span>
          </span>
        </footer>
      </section>
    </main>
  );
}

export default App;