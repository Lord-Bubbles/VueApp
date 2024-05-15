import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useManagerStore = defineStore('managerStore', () => {
  const managers = ref([]);

  const managerNames = computed(() => {
    return managers.value.map((manager) => {
      return manager.firstName + ' ' + manager.lastName;
    });
  });

  async function getManagers() {
    try {
      const response = await fetch(`/api/User?type=manager&page=1&limit=${Number('2e9')}`);
      const { users } = await response.json();
      managers.value = users;
    } catch (error) {
      console.log('An error occurred when fetching all managers: ' + error);
    }
  }

  return { managers, managerNames, getManagers };
});
