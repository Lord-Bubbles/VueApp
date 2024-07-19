<script setup>
  import { getUsers } from '@/utils/userService';
  import { keepPreviousData, useQuery } from '@tanstack/vue-query';
  import { computed } from 'vue';
  import { useAuthStore } from '@/stores/authStore';
  import { useField } from 'vee-validate';

  const props = defineProps({
    name: {
      type: String,
      required: true
    }
  });
  const authStore = useAuthStore();

  const emit = defineEmits(['update']);

  const { value: name, errorMessage } = useField(() => props.name);

  const params = {
    type: 'Manager',
    page: 1,
    limit: 1000,
    managerName: '',
    minAge: 0,
    maxAge: 0,
    email: ''
  };

  const { data } = useQuery({
    queryKey: ['users', { name: name.value }],
    queryFn: () => getUsers({ ...params, name: name.value }),
    placeholderData: keepPreviousData
  });

  const filteredUsers = computed(() =>
    data.value?.users.filter((u) => {
      if (authStore.user.accountType == 'Manager') {
        return u.id != authStore.user.id;
      }
      return u;
    })
  );

  const updateManager = (string) => {
    name.value = string;
    emit('update', string);
  };
</script>

<template>
  <div class="mb-3" v-if="data && data.users">
    <label class="form-label">Manager</label>
    <div class="position-relative">
      <input
        type="text"
        class="form-control"
        v-model="name"
        placeholder="Manager Name"
        data-bs-toggle="dropdown"
        data-bs-display="static"
      />
      <ul v-show="filteredUsers.length" class="dropdown-menu w-100 overflow-auto mb-0">
        <li
          v-for="user in filteredUsers"
          :key="user.id"
          @click="updateManager(user.firstName + ' ' + user.lastName)"
        >
          <button type="button" class="dropdown-item">
            {{ user.firstName + ' ' + user.lastName }}
          </button>
        </li>
      </ul>
    </div>
    <span class="text-danger">{{ errorMessage }}</span>
  </div>
</template>
