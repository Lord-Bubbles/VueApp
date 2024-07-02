<script setup>
  import { useMutation, useQueryClient } from '@tanstack/vue-query';
  import { ref } from 'vue';
  import { createPerformance } from '@/utils/performanceService';

  const props = defineProps({
    type: {
      required: true,
      type: String
    },
    userID: {
      type: Number,
      required: true
    }
  });
  const queryClient = useQueryClient();

  const { mutate } = useMutation({
    mutationFn: (params) => createPerformance(params),
    onSuccess: async () => {
      return await queryClient.invalidateQueries({ queryKey: ['performances'] });
    }
  });

  const formData = ref({
    rating: 0,
    goals: [''],
    improve: [''],
    well: ['']
  });
  const modal = defineModel({ required: true, type: Boolean });

  const addReview = () => {
    mutate({ ...formData.value, userID: props.userID, type: props.type });
  };
</script>

<template>
  <div
    class="modal fade"
    data-bs-backdrop="static"
    data-bs-keyboard="false"
    tabindex="-1"
    :class="{ show: modal, 'd-block': modal }"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content text-light-emphasis">
        <div class="modal-header">
          <button type="button" class="btn-close" @click="() => (modal = false)"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="addReview">
            <div class="mb-3">
              <label class="form-label">Performance Rating: </label>
              <div class="d-inline-flex flex-wrap ms-1 text-secondary">
                <!-- Add hover effect later -->
                <div
                  v-for="n in 5"
                  :key="'star-' + n"
                  class="bg-transparent border-0 p-0"
                  :class="{ 'text-black': n <= formData.rating }"
                  @click="() => (formData.rating = n)"
                >
                  &#9733;
                </div>
              </div>
            </div>
            <div class="mb-3">
              <div class="d-flex">
                <label class="form-label">Goals</label>
                <button
                  type="button"
                  class="btn btn-primary rounded-circle ms-2 btn-sm"
                  @click="() => formData.goals.push([''])"
                >
                  <i class="bi bi-plus"></i>
                </button>
              </div>
              <div
                class="d-flex justify-content-evenly mb-1"
                v-for="(_, index) in formData.goals"
                :key="'goal-' + index"
              >
                <input
                  type="text"
                  class="form-control"
                  @input="(e) => (formData.goals[index] = e.target.value)"
                  :value="formData.goals[index]"
                  required
                  minlength="1"
                />
                <button
                  type="button"
                  class="btn btn-danger rounded-circle"
                  @click="() => formData.goals.splice(index, 1)"
                >
                  <i class="bi bi-dash-lg"></i>
                </button>
              </div>
            </div>
            <div class="mb-3">
              <div class="d-flex">
                <label class="form-label">Things Did Well</label>
                <button
                  type="button"
                  class="btn btn-primary rounded-circle btn-sm ms-2"
                  @click="() => formData.well.push([''])"
                >
                  <i class="bi bi-plus"></i>
                </button>
              </div>
              <div
                class="d-flex justify-content-evenly mb-1"
                v-for="(_, index) in formData.well"
                :key="'well-' + index"
              >
                <input
                  type="text"
                  class="form-control"
                  v-model="formData.well[index]"
                  required
                  minlength="1"
                />
                <button
                  type="button"
                  class="btn btn-danger rounded-circle"
                  @click="() => formData.well.splice(index, 1)"
                >
                  <i class="bi bi-dash-lg"></i>
                </button>
              </div>
            </div>
            <div class="mb-3">
              <div class="d-flex">
                <label class="form-label">Things to Improve</label>
                <button
                  type="button"
                  class="btn btn-primary rounded-circle ms-2 btn-sm"
                  @click="() => formData.improve.push([''])"
                >
                  <i class="bi bi-plus"></i>
                </button>
              </div>
              <div
                class="d-flex justify-content-evenly mb-1"
                v-for="(_, index) in formData.improve"
                :key="'improve-' + index"
              >
                <input
                  type="text"
                  class="form-control"
                  v-model="formData.improve[index]"
                  required
                  minlength="1"
                />
                <button
                  type="button"
                  class="btn btn-danger rounded-circle ms-2"
                  @click="() => formData.improve.splice(index, 1)"
                >
                  <i class="bi bi-dash-lg"></i>
                </button>
              </div>
            </div>
            <div class="d-flex justify-content-center">
              <button type="submit" class="btn btn-primary">Create</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>