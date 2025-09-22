
/**
 * TypeScript API client for Category endpoints using fetch
 */



/**
 * DTO для передачи данных категории между слоями приложения.
 * @template TData Тип данных, хранимых в категории
 */
export interface CategoryDto<TData = unknown> {
  /** Уникальный идентификатор категории */
  id: string;
  /** Название категории */
  name: string;
  /** Идентификатор родительской категории (если есть) */
  parentId?: string | null;
  /** Данные категории (JSON) */
  data?: TData;
  /** Схема данных категории (JSON Schema) */
  schema?: any;
  /** Признак самой актуальной версии */
  isLatest: boolean;
  /** Дата загрузки/создания категории (ISO строка) */
  loadDate: string;
  /** Дочерние категории */
  children?: CategoryDto[];
}


/**
 * Интерфейс сервиса для работы с иерархическим справочником категорий
 */
export interface IHierarchicalDirectoryService {
  /** Получить все категории */
  getCategories(): Promise<CategoryDto[]>;
  /** Получить категорию по идентификатору */
  getCategory(id: string): Promise<CategoryDto>;
  /** Создать новую категорию */
  createCategory(category: Omit<CategoryDto, "id" | "children">): Promise<CategoryDto>;
  /** Обновить существующую категорию */
  updateCategory(id: string, category: Partial<CategoryDto>): Promise<CategoryDto>;
  /** Удалить категорию по идентификатору */
  deleteCategory(id: string): Promise<void>;
}


/**
 * Базовый URL для Category API
 */
const API_BASE_URL = "http://localhost:5000/api/categories";


/**
 * Обработка ответа от API
 * @param response Ответ fetch
 * @returns Десериализованный объект
 */
async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || response.statusText);
  }
  return response.json();
}


/**
 * Получить список всех категорий
 */
export async function getCategories(): Promise<CategoryDto[]> {
  const response = await fetch(`${API_BASE_URL}`);
  return handleResponse<CategoryDto[]>(response);
}


/**
 * Получить категорию по идентификатору
 * @param id Идентификатор категории
 */
export async function getCategory(id: string): Promise<CategoryDto> {
  const response = await fetch(`${API_BASE_URL}/${id}`);
  return handleResponse<CategoryDto>(response);
}


/**
 * Создать новую категорию
 * @param category Данные новой категории
 */
export async function createCategory(category: Omit<CategoryDto, "id">): Promise<CategoryDto> {
  const response = await fetch(`${API_BASE_URL}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(category),
  });
  return handleResponse<CategoryDto>(response);
}


/**
 * Обновить существующую категорию
 * @param id Идентификатор категории
 * @param category Частичные данные для обновления
 */
export async function updateCategory(id: string, category: Partial<CategoryDto>): Promise<CategoryDto> {
  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(category),
  });
  return handleResponse<CategoryDto>(response);
}


/**
 * Удалить категорию по идентификатору
 * @param id Идентификатор категории
 */
export async function deleteCategory(id: string): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: "DELETE" });
  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || response.statusText);
  }
}
