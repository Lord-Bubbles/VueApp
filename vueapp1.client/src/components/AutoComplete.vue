<script setup>
  import { getUsers } from '@/utils/userService';
  import { ref, watchEffect } from 'vue';
  import { keepPreviousData, useQuery } from '@tanstack/vue-query';
  import { useField } from 'vee-validate';

  const props = defineProps({
    user: {
      type: Object,
      default: undefined
    }
  });

  const emit = defineEmits(['update']);

  const { value: manager, errorMessage } = useField('managerName');

  const params = ref({
    type: 'Manager',
    page: 1,
    limit: 1000,
    managerName: '',
    minAge: 0,
    maxAge: 100,
    email: ''
  });

  watchEffect(() => {
    params.value.name = manager.value;
  });

  const transformData = (data) => {
    const names = data.users.map(
      (u) =>
        new Object({ fullName: u.firstName + ' ' + u.lastName, id: u.id, accountType: u.accountType })
    );
    return names.filter((u) => {
      if (props.user?.accountType == 'Manager') {
        return u.id != props.user?.id;
      }
      return u;
    });
  };

  const { data } = useQuery({
    queryKey: ['managers', params.value],
    queryFn: () => getUsers(params.value),
    placeholderData: keepPreviousData,
    enabled: !!manager.value,
    select: (data) => transformData(data)
  });

  const updateManager = (val) => {
    manager.value = val;
    emit('update', val);
  };
</script>

<template>
  <div class="mb-3">
    <label class="form-label">Manager</label>
    <div class="position-relative">
      <input
        type="text"
        class="form-control"
        v-model="manager"
        placeholder="Manager Name"
        data-bs-toggle="dropdown"
        data-bs-display="static"
      />
      <ul v-show="data?.length" class="dropdown-menu w-100 overflow-y-auto mb-0">
        <li v-for="user in data" :key="user.id">
          <button type="button" class="dropdown-item" @click="updateManager(user.fullName)">
            {{ user.fullName }}
          </button>
        </li>
      </ul>
    </div>
    <span class="text-danger">{{ errorMessage }}</span>
  </div>
</template>
