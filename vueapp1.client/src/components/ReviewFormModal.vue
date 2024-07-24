<script setup>
  import { useMutation, useQueryClient } from '@tanstack/vue-query';
  import { createPerformance } from '@/utils/performanceService';
  import { array, number, object, string } from 'yup';
  import { FieldArray, useForm, Field, ErrorMessage } from 'vee-validate';
  import { useToast } from 'vue-toastification';

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
      modal.value = false;
      return await queryClient.invalidateQueries({
        queryKey: ['performances'],
        refetchType: 'all'
      });
    },
    onError: () => {
      const toast = useToast();
      toast.error('An unexpected error has occurred while creating performance review');
    }
  });

  const modal = defineModel({ required: true, type: Boolean });

  const schema = object().shape({
    rating: number().moreThan(0, ({ more }) => `Rating must be greater than ${more}`),
    goals: array(string().required('Goal field can not be empty')).min(
      1,
      ({ min }) => `There must be at least ${min} goal`
    ),
    improve: array(string().required('Improve field can not be empty')).min(
      1,
      ({ min }) => `There must be at least ${min} point of improvement`
    ),
    well: array(string().required('Well field can not be empty')).min(
      1,
      ({ min }) => `There must be at least ${min} point you did well`
    )
  });

  const initialValues = {
    rating: 0
  };

  const { isSubmitting, handleSubmit, values, setFieldValue } = useForm({
    validationSchema: schema,
    initialValues,
    validateOnMount: false
  });

  const addReview = handleSubmit((values) => {
    mutate({ ...values, userID: props.userID, type: props.type });
  });
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
          <form @submit="addReview">
            <div class="mb-3">
              <label class="form-label">Performance Rating: </label>
              <div class="d-inline-flex flex-wrap ms-1 text-secondary">
                <!-- Add hover effect later -->
                <div
                  v-for="n in 5"
                  :key="'star-' + n"
                  class="bg-transparent border-0 p-0"
                  :class="{ 'text-black': n <= values.rating }"
                  @click="setFieldValue('rating', n)"
                >
                  &#9733;
                </div>
              </div>
              <ErrorMessage class="d-block text-danger" name="rating" />
            </div>
            <div class="mb-3">
              <FieldArray name="goals" v-slot="{ fields, push, remove }">
                <div class="d-flex">
                  <label class="form-label">Goals</label>
                  <button
                    type="button"
                    class="btn btn-primary rounded-circle ms-2 btn-sm"
                    @click="push('')"
                  >
                    <i class="bi bi-plus"></i>
                  </button>
                </div>
                <div v-for="(field, index) in fields" :key="field.key" class="mb-1">
                  <div class="d-flex justify-content-evenly">
                    <Field type="text" class="form-control" :name="`goals[${index}]`" />
                    <button
                      type="button"
                      class="btn btn-danger rounded-circle"
                      @click="remove(index)"
                    >
                      <i class="bi bi-dash-lg"></i>
                    </button>
                  </div>
                  <ErrorMessage :name="`goals[${index}]`" class="text-danger" />
                </div>
              </FieldArray>
              <ErrorMessage name="goals" class="text-danger" />
            </div>
            <div class="mb-3">
              <FieldArray name="well" v-slot="{ fields, push, remove }">
                <div class="d-flex">
                  <label class="form-label">Things Did Well</label>
                  <button
                    type="button"
                    class="btn btn-primary rounded-circle btn-sm ms-2"
                    @click="push('')"
                  >
                    <i class="bi bi-plus"></i>
                  </button>
                </div>
                <div v-for="(field, index) in fields" :key="field.key" class="mb-1">
                  <div class="d-flex justify-content-evenly">
                    <Field type="text" class="form-control" :name="`well[${index}]`" />
                    <button
                      type="button"
                      class="btn btn-danger rounded-circle"
                      @click="remove(index)"
                    >
                      <i class="bi bi-dash-lg"></i>
                    </button>
                  </div>
                  <ErrorMessage :name="`well[${index}]`" class="text-danger" />
                </div>
              </FieldArray>
              <ErrorMessage name="well" class="text-danger" />
            </div>
            <div class="mb-3">
              <FieldArray name="improve" v-slot="{ fields, push, remove }">
                <div class="d-flex">
                  <label class="form-label">Things to Improve</label>
                  <button
                    type="button"
                    class="btn btn-primary rounded-circle ms-2 btn-sm"
                    @click="push('')"
                  >
                    <i class="bi bi-plus"></i>
                  </button>
                </div>
                <div class="mb-1" v-for="(field, index) in fields" :key="field.key">
                  <div class="d-flex justify-content-evenly mb-1">
                    <Field type="text" class="form-control" :name="`improve[${index}]`" />
                    <button
                      type="button"
                      class="btn btn-danger rounded-circle ms-2"
                      @click="remove(index)"
                    >
                      <i class="bi bi-dash-lg"></i>
                    </button>
                  </div>
                  <ErrorMessage :name="`improve[${index}]`" class="text-danger" />
                </div>
              </FieldArray>
              <ErrorMessage name="improve" class="text-danger" />
            </div>
            <div class="d-flex justify-content-center">
              <button type="submit" class="btn btn-primary">
                {{ isSubmitting ? 'Creating...' : 'Create' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
