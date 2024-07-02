<script setup>
  import { getUsers } from '@/utils/userService';
  import { keepPreviousData, useQuery } from '@tanstack/vue-query';
  import { computed, ref, watchEffect } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import { useAuthStore } from '@/stores/authStore';

  const manager = defineModel({ required: true, type: String });
  const route = useRoute();
  const router = useRouter();
  const name = ref(manager.value);
  const authStore = useAuthStore();

  const params = {
    type: 'Manager',
    page: 1,
    limit: 1000,
    managerName: '',
    minAge: 0,
    maxAge: 0,
    email: ''
  };

  watchEffect(() => (name.value = manager.value));

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
      <ul v-show="data.users.length" class="dropdown-menu w-100 overflow-auto mb-0">
        <li
          v-for="user in filteredUsers"
          :key="user.id"
          @click="
            {
              const fullName = user.firstName + ' ' + user.lastName;
              manager = fullName;
              name = fullName;
              const query = Object.assign({}, route.query);
              query.managerName = fullName;
              router.replace({
                query
              });
            }
          "
        >
          <button type="button" class="dropdown-item">
            {{ user.firstName + ' ' + user.lastName }}
          </button>
        </li>
      </ul>
    </div>
  </div>
</template>
