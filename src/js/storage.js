/**
 * Gerenciamento de Persistência com IndexedDB e LocalStorage
 * Salva assinaturas de áudio, gravações e configurações dos gatilhos
 */

const DB_NAME = 'PokeSentinelDB';
const DB_VERSION = 1;
const STORE_TRIGGERS = 'triggers';
const STORE_SETTINGS = 'settings';

class SentinelStorage {
  constructor() {
    this.db = null;
    this.isReady = false;
  }

  async init() {
    return new Promise((resolve, reject) => {
      const request = indexedDB.open(DB_NAME, DB_VERSION);

      request.onupgradeneeded = (event) => {
        const db = event.target.result;
        if (!db.objectStoreNames.contains(STORE_TRIGGERS)) {
          db.createObjectStore(STORE_TRIGGERS, { keyPath: 'id' });
        }
        if (!db.objectStoreNames.contains(STORE_SETTINGS)) {
          db.createObjectStore(STORE_SETTINGS, { keyPath: 'key' });
        }
      };

      request.onsuccess = (event) => {
        this.db = event.target.result;
        this.isReady = true;
        resolve(this.db);
      };

      request.onerror = (event) => {
        console.error('Erro ao abrir IndexedDB:', event.target.error);
        reject(event.target.error);
      };
    });
  }

  async getAllTriggers() {
    if (!this.db) await this.init();
    return new Promise((resolve, reject) => {
      const transaction = this.db.transaction([STORE_TRIGGERS], 'readonly');
      const store = transaction.objectStore(STORE_TRIGGERS);
      const request = store.getAll();

      request.onsuccess = () => resolve(request.result || []);
      request.onerror = (e) => reject(e.target.error);
    });
  }

  async saveTrigger(trigger) {
    if (!this.db) await this.init();
    return new Promise((resolve, reject) => {
      const transaction = this.db.transaction([STORE_TRIGGERS], 'readwrite');
      const store = transaction.objectStore(STORE_TRIGGERS);
      const request = store.put(trigger);

      request.onsuccess = () => resolve(true);
      request.onerror = (e) => reject(e.target.error);
    });
  }

  async deleteTrigger(id) {
    if (!this.db) await this.init();
    return new Promise((resolve, reject) => {
      const transaction = this.db.transaction([STORE_TRIGGERS], 'readwrite');
      const store = transaction.objectStore(STORE_TRIGGERS);
      const request = store.delete(id);

      request.onsuccess = () => resolve(true);
      request.onerror = (e) => reject(e.target.error);
    });
  }

  async saveSetting(key, value) {
    if (!this.db) await this.init();
    return new Promise((resolve, reject) => {
      const transaction = this.db.transaction([STORE_SETTINGS], 'readwrite');
      const store = transaction.objectStore(STORE_SETTINGS);
      const request = store.put({ key, value });

      request.onsuccess = () => resolve(true);
      request.onerror = (e) => reject(e.target.error);
    });
  }

  async getSetting(key, defaultValue = null) {
    if (!this.db) await this.init();
    return new Promise((resolve) => {
      const transaction = this.db.transaction([STORE_SETTINGS], 'readonly');
      const store = transaction.objectStore(STORE_SETTINGS);
      const request = store.get(key);

      request.onsuccess = () => {
        resolve(request.result ? request.result.value : defaultValue);
      };
      request.onerror = () => resolve(defaultValue);
    });
  }
}

window.sentinelStorage = new SentinelStorage();
